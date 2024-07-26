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
    [SerializeField] LayerMask BlockRay;
    [SerializeField] float PistolShotDistance;
    [SerializeField] float ARShotDistance;
    [SerializeField] float SniperShotDistance;
    [SerializeField] float PistolFirerate;
    [SerializeField] float ARFirerate;
    [SerializeField] float SniperFirerate;
    [SerializeField] GameObject PistolLaserEndLocation;
    [SerializeField] GameObject ARLaserEndLocation;
    [SerializeField] GameObject SniperLaserEndLocation;
    [SerializeField] string ScrewDriverTag;
    [SerializeField] string KeyCardTag;
    [SerializeField] string WireCutterTag;
    [SerializeField] string ARTag;
    [SerializeField] string SniperTag;
    [SerializeField] string BossTag;
    [SerializeField] LineRenderer LaserLine;
    [SerializeField] float LaserShowTime;
    [SerializeField] GameObject PistolSprite;
    [SerializeField] GameObject ARSprite;
    [SerializeField] GameObject SniperSprite;
    public bool PistolAccquired;
    public bool ARAccquired;
    public bool SniperAccquired;
    public bool ScrewDriver;
    public bool KeyCard;
    public bool WireCutter;
    private float ShotDistance;
    private float FirerateTime;
    private bool ReadyToFire;
    public bool Damageable;
    private int CurrentHealth;
    private Vector2 MoveDirection;
    private Vector2 MousePosition;
    private int EquippedWeapon;
    private Vector3 LaserEndPosition;

    // Start is called before the first frame update
    void Start()
    {
        Damageable = true;
        CurrentHealth = FindObjectOfType<LevelController>().HealthCarriedBetweenLevels;
        Cursor.lockState = CursorLockMode.Confined;
        ScrewDriver = FindObjectOfType<LevelController>().ScrewDriverAcquired;
        LaserLine.enabled = false;
        EquippedWeapon = 1;
        PistolAccquired = true;
        ARAccquired = FindObjectOfType<LevelController>().ARAcquired;
        SniperAccquired = FindObjectOfType<LevelController>().SniperAcquired;
        PistolSprite.SetActive(true);
        ARSprite.SetActive(false);
        SniperSprite.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (EquippedWeapon)
        {
            case 1:
                LaserEndPosition = PistolLaserEndLocation.transform.position; break;
            case 2:
                LaserEndPosition = ARLaserEndLocation.transform.position; break;
            case 3:
                LaserEndPosition = SniperLaserEndLocation.transform.position; break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && PistolAccquired && EquippedWeapon != 1)
        {
            EquippedWeapon = 1;
            FirerateTime = 0.5f;
            PistolSprite.SetActive(true);
            ARSprite.SetActive(false);
            SniperSprite.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && ARAccquired && EquippedWeapon != 2)
        {
            EquippedWeapon = 2;
            FirerateTime = 0.5f;
            PistolSprite.SetActive(false);
            ARSprite.SetActive(true);
            SniperSprite.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && SniperAccquired && EquippedWeapon != 3)
        {
            EquippedWeapon = 3;
            FirerateTime = 0.5f;
            PistolSprite.SetActive(false);
            ARSprite.SetActive(false);
            SniperSprite.SetActive(true);
        }


        LaserLine.SetPosition(0, Firepoint.transform.position);
        LaserLine.SetPosition(1, LaserEndPosition);
        PlayerControl();
        ShootPistolLaser();
        if (FirerateTime <= 0)
        {
            ReadyToFire = true;
        }
        else
        {
            FirerateTime -= Time.deltaTime;
        }
    }

    private void ShootPistolLaser()
    {
        if (Input.GetKey(ShootKey) && ReadyToFire == true)
        {
            switch (EquippedWeapon)
            {
                case 1:
                    FirerateTime = PistolFirerate; ShotDistance = PistolShotDistance; break;
                case 2:
                    FirerateTime = ARFirerate; ShotDistance = ARShotDistance; break;
                case 3:
                    FirerateTime = SniperFirerate; ShotDistance = SniperShotDistance; break;
            }



            LaserLine.enabled = true;
            ReadyToFire = false;
            Ray2D ray = new Ray2D();
            ray.origin = Firepoint.transform.position;
            ray.direction = MousePosition - ray.origin;
            RaycastHit2D castResult = Physics2D.Raycast(ray.origin, ray.direction.normalized, ShotDistance);
            FindObjectOfType<AudioManager>().Play("Laser Blast");
            StartCoroutine(DisableLaser());
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
        if (aimAngle < -180 || aimAngle > 0)
        {
            PistolSprite.transform.localPosition = new Vector3(0.05f, 0.57f, 0);
            PistolSprite.GetComponent<SpriteRenderer>().flipY = true;
            ARSprite.transform.localPosition = new Vector3(-0.11f, 0.22f, 0);
            ARSprite.GetComponent<SpriteRenderer>().flipY = true;
            SniperSprite.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            PistolSprite.transform.localPosition = new Vector3(0.86f, 0.57f, 0);
            PistolSprite.GetComponent<SpriteRenderer>().flipY = false;
            ARSprite.transform.localPosition = new Vector3(0.988f, 0.22f, 0);
            ARSprite.GetComponent<SpriteRenderer>().flipY = false;
            SniperSprite.GetComponent<SpriteRenderer>().flipY = false;
        }

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
        if (collision.CompareTag(ScrewDriverTag)) { ScrewDriver = true; collision.transform.GetComponent<Objects>().Death(); FindObjectOfType<AudioManager>().Play("Pickup"); }
        if (collision.CompareTag(KeyCardTag)) { KeyCard = true; collision.transform.GetComponent<Objects>().Death(); FindObjectOfType<AudioManager>().Play("Pickup"); }
        if (collision.CompareTag(WireCutterTag)) { WireCutter = true; collision.transform.GetComponent<Objects>().Death(); FindObjectOfType<AudioManager>().Play("Pickup"); }
        if (collision.CompareTag(ARTag)) { ARAccquired = true; collision.transform.GetComponent<Objects>().Death(); FindObjectOfType<AudioManager>().Play("Pickup"); }
        if (collision.CompareTag(SniperTag)) { SniperAccquired = true; collision.transform.GetComponent<Objects>().Death(); FindObjectOfType<AudioManager>().Play("Pickup"); }
        if (collision.CompareTag(LevelChangeTag)) { FindObjectOfType<LevelController>().LevelChangerActive(); }
        if (collision.CompareTag("Bomb")) { FindObjectOfType<LevelController>().GameOver(true); }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        FindObjectOfType<LevelController>()?.LevelChangerInActive();
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

    IEnumerator DisableLaser()
    {
        yield return new WaitForSeconds(LaserShowTime);
        LaserLine.enabled = false;
    }
}

