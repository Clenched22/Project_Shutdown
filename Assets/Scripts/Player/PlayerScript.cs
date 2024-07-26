using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    public float MoveSpeed;
    [SerializeField] GameObject PistolFirepoint;
    [SerializeField] GameObject ARFirepoint;
    [SerializeField] GameObject SniperFirepoint;
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
    [SerializeField] LineRenderer PistolLaserLine;
    [SerializeField] LineRenderer ARLaserLine;
    [SerializeField] LineRenderer SniperLaserLine;
    private LineRenderer ActiveLine;
    [SerializeField] float LaserShowTime;
    [SerializeField] GameObject PistolSprite;
    [SerializeField] GameObject ARSprite;
    [SerializeField] GameObject SniperSprite;
    [SerializeField] float PistolDamage;
    [SerializeField] float ARDamage;
    [SerializeField] float SniperDamage;
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
    private float CurrentHealth;
    private Vector2 MoveDirection;
    private Vector2 MousePosition;
    private int EquippedWeapon;
    private Vector3 LaserEndPosition;
    private float ActualDamageDealt;
    private Vector3 LaserStartPosition;

    // Start is called before the first frame update
    void Start()
    {
        Damageable = true;
        CurrentHealth = FindObjectOfType<LevelController>().HealthCarriedBetweenLevels;
        Cursor.lockState = CursorLockMode.Confined;
        ScrewDriver = FindObjectOfType<LevelController>().ScrewDriverAcquired;
        EquippedWeapon = 1;
        PistolAccquired = true;
        ARAccquired = FindObjectOfType<LevelController>().ARAcquired;
        SniperAccquired = FindObjectOfType<LevelController>().SniperAcquired;
        PistolSprite.SetActive(true);
        ARSprite.SetActive(false);
        SniperSprite.SetActive(false);
        LaserStartPosition = PistolFirepoint.transform.position;
        PistolLaserLine.enabled = false;
        ARLaserLine.enabled = false;
        SniperLaserLine.enabled = false;
        ActiveLine = PistolLaserLine;
    }

    // Update is called once per frame
    void Update()
    {

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
        switch (EquippedWeapon)
        {
            case 0:
                Debug.LogError("HELP");
                break;
            case 1:
                ARLaserLine.enabled = false; SniperLaserLine.enabled = false; ActiveLine = PistolLaserLine; LaserStartPosition = PistolFirepoint.transform.position; LaserEndPosition = PistolLaserEndLocation.transform.position; break;
            case 2:
                PistolLaserLine.enabled = false; SniperLaserLine.enabled = false; ActiveLine = ARLaserLine; LaserStartPosition = ARFirepoint.transform.position; LaserEndPosition = ARLaserEndLocation.transform.position; break;
            case 3:
                ARLaserLine.enabled = false; PistolLaserLine.enabled = false; ActiveLine = SniperLaserLine; LaserStartPosition = SniperFirepoint.transform.position; LaserEndPosition = SniperLaserEndLocation.transform.position; break;
        }

        PistolLaserLine.SetPosition(0, LaserStartPosition);
        PistolLaserLine.SetPosition(1, LaserEndPosition);
        ARLaserLine.SetPosition(0, LaserStartPosition);
        ARLaserLine.SetPosition(1, LaserEndPosition);
        SniperLaserLine.SetPosition(0, LaserStartPosition);
        SniperLaserLine.SetPosition(1, LaserEndPosition);

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
            string AudioName = null;
            switch (EquippedWeapon)
            {
                case 0: Debug.LogError("HELP"); break;
                case 1:
                    PistolLaserLine.enabled = true; 
                    FirerateTime = PistolFirerate; 
                    ShotDistance = PistolShotDistance; 
                    ActualDamageDealt = PistolDamage; 
                    AudioName = "PistolLaser"; 
                    break;
                case 2:
                    ARLaserLine.enabled = true; 
                    FirerateTime = ARFirerate; 
                    ShotDistance = ARShotDistance; 
                    ActualDamageDealt = ARDamage; 
                    AudioName = "ARLaser"; 
                    break;
                case 3:
                    SniperLaserLine.enabled = true; 
                    FirerateTime = SniperFirerate; 
                    ShotDistance = SniperShotDistance; 
                    ActualDamageDealt = SniperDamage; 
                    AudioName = "SniperLaser"; 
                    break;
            }

            ReadyToFire = false;
            Ray2D ray = new Ray2D();
            ray.origin = LaserStartPosition;
            ray.direction = MousePosition - ray.origin;
            RaycastHit2D castResult = Physics2D.Raycast(ray.origin, ray.direction.normalized, ShotDistance);
            //FindObjectOfType<AudioManager>().Play(AudioName);
            StartCoroutine(DisableLaser());
            if (castResult.transform.CompareTag(EnemyTag) && castResult.distance <= ShotDistance)
                {
                    castResult.transform.GetComponent<Regular>().HealthDecrease(ActualDamageDealt);
                }
            if (castResult.transform.CompareTag(BossTag) && castResult.distance <= ShotDistance)
                {
                    FindObjectOfType<Boss>().DecreaseHealth(ActualDamageDealt);
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
            SniperSprite.transform.localPosition = new Vector3(-0.25f, 0.05f, 0);
            SniperSprite.GetComponent<SpriteRenderer>().flipY = true;
        }
        else
        {
            PistolSprite.transform.localPosition = new Vector3(0.86f, 0.57f, 0);
            PistolSprite.GetComponent<SpriteRenderer>().flipY = false;
            ARSprite.transform.localPosition = new Vector3(0.988f, 0.22f, 0);
            ARSprite.GetComponent<SpriteRenderer>().flipY = false;
            SniperSprite.transform.localPosition = new Vector3(1.08f, 0.05f, 0);
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
        FindObjectOfType<LevelController>().HealthCarriedBetweenLevels = CurrentHealth;


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

    public void DecreaseHealth(float healthDecrease)
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
        else if (CurrentHealth <= 0) { FindObjectOfType<LevelController>().GameOver(false); FindObjectOfType<AudioManager>().Play("PlayerDeath"); }
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
        PistolLaserLine.enabled = false;
        ARLaserLine.enabled = false; 
        SniperLaserLine.enabled = false;
    }
}

