using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionToTarget = TrackingTarget.position - transform.position;
        transform.up = directionToTarget;
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
        if (AbleToShoot)
        {
            //Audio Spit Green goo like smth
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
        if (collision.transform.CompareTag("Player"))
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
    }

    private void Death()
    {
        FindObjectOfType<LevelController>().EnemyDeathIndexReset(ESI.LevelIndex, ESI.SpawnIndex);
        FindObjectOfType<AudioManager>().Play("BasicEnemyDeath");
        Destroy(gameObject);
    }
}
