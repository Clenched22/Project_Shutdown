using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] float MoveSpeed;
    [SerializeField] GameObject Firepoint;
    [SerializeField] Rigidbody2D RotationPoint;
    [SerializeField] Rigidbody2D RefRigidbody;
    [SerializeField] KeyCode ShootKey;
    [SerializeField] string LevelForwardTag;
    [SerializeField] string LevelBackwardTag;
    [SerializeField] float FlashDelay;
    [SerializeField] string EnemyTag;
    [SerializeField] float ShotDistance;
    [SerializeField] LayerMask BlockRay;
    private bool Damageable;
    private int CurrentHealth;
    private Vector2 MoveDirection;
    private Vector2 MousePosition;


    // Start is called before the first frame update
    void Start()
    {
        //CurrentHealth = FindObjectOfType<LevelController>().HealthCarriedBetweenLevels;
        Cursor.lockState = CursorLockMode.Confined;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerControl();
        ShootWeapon();
    }

    private void ShootWeapon()
    {
        if (Input.GetKeyDown(ShootKey))
        {
            Debug.Log("Fire");
            Ray2D ray = new Ray2D();
            ray.origin = Firepoint.transform.position;
            ray.direction = MousePosition - ray.origin;
            RaycastHit2D castResult = Physics2D.Raycast(ray.origin, ray.direction.normalized, ShotDistance);
                if (castResult.transform.CompareTag(EnemyTag) && castResult.distance <= ShotDistance)
                {
                    Destroy(castResult.transform.gameObject);
                }
        }
    }

    private void FixedUpdate()
    {
        RefRigidbody.velocity = MoveDirection;
        RotationPoint.velocity = MoveDirection;
        Vector2 lookDirection = MousePosition - RefRigidbody.position;
        float aimAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        RotationPoint.rotation = aimAngle;
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
}

