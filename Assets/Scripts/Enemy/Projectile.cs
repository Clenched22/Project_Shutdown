using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UI;

public class Projectile : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool IsChasing;
    [SerializeField] float ChaseDistance;
    public EnemySpawnInformation ESI;
    [SerializeField] Rigidbody2D RB;
    [SerializeField] float PushbackForce;
    [SerializeField] GameObject EnemyProjectile;
    [SerializeField] GameObject FirePoint;
    [SerializeField] LayerMask ObstacleMask;
    [SerializeField] float DetectionRange;
    [SerializeField] float FirerateTime;
    private bool AbleToShoot;
    private float FireRateTimeCountdown;
    [SerializeField] Slider HealthSlider;
    [SerializeField] GameObject deathPartical;
    [SerializeField] GameObject hitPartical;
    [SerializeField] GameObject RotationPoint;
    private bool Damageable;

    // Start is called before the first frame update
    void Start()
    {
        if (RB == null)
        {
            RB = GetComponent<Rigidbody2D>();
        }
        if (TrackingTarget == null)
        {
            TrackingTarget = FindObjectOfType<PlayerScript>().gameObject.transform;
        }
        AbleToShoot = false;
        FireRateTimeCountdown = FirerateTime;
        ESI.Health = ESI.MaxHealth;
        Damageable = true;
    }

    // Update is called once per frame
    void Update()
    {
        HealthSlider.value = ESI.Health / ESI.MaxHealth;
        Vector2 directionToTarget = TrackingTarget.position - transform.position;
        RotationPoint.transform.up = directionToTarget;
        if (IsChasing)
        {
            if (Vector2.Distance(transform.position, TrackingTarget.position) > ChaseDistance)
            {
                IsChasing = false;
            }
            if (transform.position.x > TrackingTarget.position.x)
            {
                transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
            }
            else if (transform.position.x < TrackingTarget.position.x)
            {
                transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
            }
            if (transform.position.y > TrackingTarget.position.y)
            {
                transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
            }
            else if (transform.position.y < TrackingTarget.position.y)
            {
                transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, TrackingTarget.position) < ChaseDistance)
            {
                IsChasing = true;
            }
        }
        FireRateTimeCountdown -= Time.deltaTime;
        if (FireRateTimeCountdown <= 0)
        {
            bool targetInSight = TargetInSight();
            if (targetInSight)
            {
                AbleToShoot = true;
                Shoot();
            }
            FireRateTimeCountdown = FirerateTime;
        }
        else { AbleToShoot = false; }
    }

    private void Shoot()
    {
        if (AbleToShoot && Damageable)
        {
            FindObjectOfType<AudioManager>().Play("EnemyShoot");
            Vector2 spawnPosition = FirePoint.transform.position;
            Quaternion spawnRotation = FirePoint.transform.rotation;
            GameObject firedProjectile = Instantiate(EnemyProjectile, spawnPosition, spawnRotation);
            firedProjectile.GetComponent<ProjectileMovement>().Damage = ESI.DamageDealt;
        }
    }

    private bool TargetInSight()
    {
        Vector2 directionToTarget = TrackingTarget.position - FirePoint.transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget <= DetectionRange)
        {
            RaycastHit2D castResult = Physics2D.Raycast(FirePoint.transform.position, directionToTarget, distanceToTarget);
            if (castResult.transform.CompareTag("Player"))
            {
                return true;
            }
            else { return false; }
        }
        else { return false; }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && Damageable)
        {
            FindObjectOfType<PlayerScript>().DecreaseHealth(ESI.DamageDealt);
            Vector2 difference = (transform.position - collision.transform.position).normalized;
            Vector2 force = difference * PushbackForce;
            RB.AddForce(force, ForceMode2D.Impulse);
        }
    }
    public void HealthDecrease(float damageTaken)
    {
        ESI.Health -= damageTaken;
        if (ESI.Health <= 0) { Death(); }
        else { Instantiate(hitPartical, transform.position, Quaternion.identity); }
    }

    private void Death()
    {
        FindObjectOfType<LevelController>().EnemyDeathIndexReset(ESI.LevelIndex, ESI.SpawnIndex);
        FindObjectOfType<AudioManager>().Play("ProjectileEnemyDeath");
        Instantiate(deathPartical, transform.position, Quaternion.identity);
        Damageable = false;
        StartCoroutine(BlackDeath());
    }

    IEnumerator BlackDeath()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
