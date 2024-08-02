using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Boss : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool IsChasing;
    [SerializeField] float ChaseDistance;
    public EnemySpawnInformation ESI;
    [SerializeField] Rigidbody2D RB;
    [SerializeField] float PushbackForce;
    [SerializeField] Slider HealthSlider;
    [SerializeField] Animator BossEnemyAnimator;
    [SerializeField] GameObject hitParticle;
    [SerializeField] GameObject deathParticle;
    private bool Damageable;

    void Start()
    {
        if (TrackingTarget == null)
        {
            TrackingTarget = FindObjectOfType<PlayerScript>().gameObject.transform;
        }
        RB = GetComponent<Rigidbody2D>();
        ESI.Health = ESI.MaxHealth;
        Damageable = true;
    }
    void Update()
    {
        HealthSlider.value = ESI.Health/ESI.MaxHealth;

        if (Damageable)
        {
            if (IsChasing)
            {
                if (Vector2.Distance(transform.position, TrackingTarget.position) > ChaseDistance)
                {
                    IsChasing = false;
                    BossEnemyAnimator.SetBool("BossWalkUp", false);
                    BossEnemyAnimator.SetBool("BossWalkDown", false);
                    BossEnemyAnimator.SetBool("BossWalkLeft", false);
                    BossEnemyAnimator.SetBool("BossWalkRight", false);
                    return;
                }
                if (transform.position.x > TrackingTarget.position.x)
                {
                    transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
                    BossEnemyAnimator.SetBool("BossWalkUp", false);
                    BossEnemyAnimator.SetBool("BossWalkDown", false);
                    BossEnemyAnimator.SetBool("BossWalkLeft", true);
                    BossEnemyAnimator.SetBool("BossWalkRight", false);
                }
                else if (transform.position.x < TrackingTarget.position.x)
                {
                    transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
                    BossEnemyAnimator.SetBool("BossWalkUp", false);
                    BossEnemyAnimator.SetBool("BossWalkDown", false);
                    BossEnemyAnimator.SetBool("BossWalkLeft", false);
                    BossEnemyAnimator.SetBool("BossWalkRight", true);
                }
                if (transform.position.y > TrackingTarget.position.y)
                {
                    transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
                    BossEnemyAnimator.SetBool("WalkingUp", false);
                    BossEnemyAnimator.SetBool("WalkingDown", true);
                    BossEnemyAnimator.SetBool("WalkingLeft", false);
                    BossEnemyAnimator.SetBool("WalkingRight", false);
                }
                else if (transform.position.y < TrackingTarget.position.y)
                {
                    transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
                    BossEnemyAnimator.SetBool("BossWalkUp", true);
                    BossEnemyAnimator.SetBool("BossWalkDown", false);
                    BossEnemyAnimator.SetBool("BossWalkLeft", false);
                    BossEnemyAnimator.SetBool("BossWalkRight", false);
                }
            }
            else
            {
                if (Vector2.Distance(transform.position, TrackingTarget.position) < ChaseDistance)
                {
                    IsChasing = true;
                }
            }
        }
    }

    public void DecreaseHealth(float damageDealt)
    {
        ESI.Health -= damageDealt;
        if (ESI.Health <= 0)
        {
            Death();
        }
        else { Instantiate(hitParticle, transform.position, Quaternion.identity); }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && Damageable)
        {
            FindObjectOfType<PlayerScript>().DecreaseHealth(2);
            Vector2 difference = (transform.position - collision.transform.position).normalized;
            Vector2 force = difference * PushbackForce;
            RB.AddForce(force, ForceMode2D.Impulse);
        }
    }

    public void Death()
    {
        FindObjectOfType<LevelController>().EnemyDeathIndexReset(ESI.LevelIndex, ESI.SpawnIndex);
        FindObjectOfType<AudioManager>().Play("BossDeath");
        Damageable = false;
        Instantiate(deathParticle, transform.position, Quaternion.identity);
        Destroy(gameObject);
        //StartCoroutine(BlackDeath());
    }

    IEnumerator BlackDeath()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
