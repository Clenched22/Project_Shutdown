using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    public float MoveSpeed;
    [SerializeField] GameObject Firepoint;
    [SerializeField] GameObject RotationPoint;
    [SerializeField] Rigidbody2D RefRigidbody;
    [SerializeField] KeyCode ShootKey;
    [SerializeField] string LevelChangeTag;
    [SerializeField] float FlashDelay;
    [SerializeField] string EnemyTag;
    [SerializeField] float ShotDistance;
    [SerializeField] LayerMask BlockRay;
    [SerializeField] float PistolFireate;
    [SerializeField] GameObject PistolLaser;
    [SerializeField] GameObject SpawnPosition;
    [SerializeField] string Item1Tag;
    [SerializeField] string BossTag;
    public bool Item1Equipped;
    private float FirerateTime;
    private bool ReadyToFire;
    public bool Damageable;
    private int CurrentHealth;
    private Vector2 MoveDirection;
    private Vector2 MousePosition;


    // Start is called before the first frame update
    void Start()
    {
        Damageable = true;
        CurrentHealth = FindObjectOfType<LevelController>().HealthCarriedBetweenLevels;
        Cursor.lockState = CursorLockMode.Confined;
        Item1Equipped = FindObjectOfType<LevelController>().Item1Acquired;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
        ShootWeapon();
        Debug.Log(FirerateTime);
        if (FirerateTime <= 0)
        {
            ReadyToFire = true;
        }
        else
        {
            FirerateTime -= Time.deltaTime;
        }
    }

    private void ShootWeapon()
    {
        if (Input.GetKey(ShootKey) && ReadyToFire == true)
        {
            Vector3 spawnPosition = SpawnPosition.transform.position;
            spawnPosition.z = 2;
            Instantiate(PistolLaser, spawnPosition, RotationPoint.transform.rotation);
            FirerateTime = PistolFireate;
            ReadyToFire = false;
            Debug.Log("Fire");
            Ray2D ray = new Ray2D();
            ray.origin = Firepoint.transform.position;
            ray.direction = MousePosition - ray.origin;
            RaycastHit2D castResult = Physics2D.Raycast(ray.origin, ray.direction.normalized, ShotDistance);
            FindObjectOfType<AudioManager>().Play("Laser Blast");
            if (castResult.transform.CompareTag(EnemyTag) && castResult.distance <= ShotDistance)
                {
                    castResult.transform.GetComponent<Regular>().Death();
                    FindObjectOfType<AudioManager>().Play("Death");
                }
            if (castResult.transform.CompareTag(BossTag) && castResult.distance <= ShotDistance)
                {
                    FindObjectOfType<Boss>().DecreaseHealth();
                    FindObjectOfType<AudioManager>().Play("Death");
                }
        }
    }

    private void FixedUpdate()
    {
       
        RefRigidbody.velocity = MoveDirection;
        Vector2 lookDirection = MousePosition - RefRigidbody.position;
        float aimAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        RotationPoint.transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }

    private void PlayerControl()
    {
        Vector2 inputVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { inputVector += new Vector2(0, 1); /*AudioManager.instance.PlayWalkSound();*/ }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { inputVector += new Vector2(0, -1); /*AudioManager.instance.PlayWalkSound();*/ }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { inputVector += new Vector2(-1, 0); /*AudioManager.instance.PlayWalkSound();*/ }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { inputVector += new Vector2(1, 0); /*AudioManager.instance.PlayWalkSound(); */}
        
        inputVector.Normalize();
        MoveDirection = inputVector * MoveSpeed;
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //FindObjectOfType<LevelController>().HealthCarriedBetweenLevels = currentHealth;


        Vector2 movement;
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Item1Tag)) { Item1Equipped = true; collision.transform.GetComponent<Objects>().Death(); FindObjectOfType<AudioManager>().Play("Pickup"); }
        if (collision.CompareTag(LevelChangeTag)) { FindObjectOfType<LevelController>().LevelChangerActive(); }
        if (collision.CompareTag("Bomb")) { FindObjectOfType<LevelController>().GameOver(true); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        FindObjectOfType<LevelController>().LevelChangerInActive();
    }

    public void DecreaseHealth(int healthDecrease)
    {
        if (CurrentHealth > 0)
        {
            if (Damageable)
            {
                Damageable = false;
                CurrentHealth -= healthDecrease;
                FindObjectOfType<LevelController>().HealthCarriedBetweenLevels = CurrentHealth;
                StartCoroutine(HealthDelay());
            }
        }
        else if (CurrentHealth <= 0) { FindObjectOfType<LevelController>().GameOver(false); }
    }

    IEnumerator HealthDelay()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1);
        Damageable = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        Time.timeScale = 1;
    }
}

