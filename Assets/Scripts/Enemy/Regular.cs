using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Regular : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool IsChasing;
    [SerializeField] float ChaseDistance;
    public EnemySpawnInformation ESI;
    [SerializeField] Rigidbody2D RB;
    [SerializeField] float PushbackForce;
    [SerializeField] Slider HealthSlider;
    [SerializeField] GameObject deathPartical;
    [SerializeField] GameObject hitPartical;
    [SerializeField] Animator BasicEnemyAnimator;
    private bool Damageable;

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
        ESI.Health = ESI.MaxHealth;
        Damageable = true;
    }
    void Update()
    {
        HealthSlider.value = ESI.Health / ESI.MaxHealth;

        if (IsChasing)
        {
            if (Vector2.Distance(transform.position, TrackingTarget.position) > ChaseDistance)
            {
                IsChasing = false;
                BasicEnemyAnimator.SetBool("WalkingUp", false);
                BasicEnemyAnimator.SetBool("WalkingDown", false);
                BasicEnemyAnimator.SetBool("WalkingLeft", false);
                BasicEnemyAnimator.SetBool("WalkingRight", false);
            }
            if (transform.position.x > TrackingTarget.position.x)
            {
                transform.position += Vector3.left * MoveSpeed * Time.deltaTime;
                BasicEnemyAnimator.SetBool("WalkingUp", false);
                BasicEnemyAnimator.SetBool("WalkingDown", false);
                BasicEnemyAnimator.SetBool("WalkingLeft", true);
                BasicEnemyAnimator.SetBool("WalkingRight", false);
            }
            else if (transform.position.x < TrackingTarget.position.x)
            {
                transform.position += Vector3.right * MoveSpeed * Time.deltaTime;
                BasicEnemyAnimator.SetBool("WalkingUp", false);
                BasicEnemyAnimator.SetBool("WalkingDown", false);
                BasicEnemyAnimator.SetBool("WalkingLeft", false);
                BasicEnemyAnimator.SetBool("WalkingRight", true);
            }
            if (transform.position.y > TrackingTarget.position.y)
            {
                transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
                BasicEnemyAnimator.SetBool("WalkingUp", false);
                BasicEnemyAnimator.SetBool("WalkingDown", true);
                BasicEnemyAnimator.SetBool("WalkingLeft", false);
                BasicEnemyAnimator.SetBool("WalkingRight", false);
            }
            else if (transform.position.y < TrackingTarget.position.y)
            {
                transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
                BasicEnemyAnimator.SetBool("WalkingUp", true);
                BasicEnemyAnimator.SetBool("WalkingDown", false);
                BasicEnemyAnimator.SetBool("WalkingLeft", false);
                BasicEnemyAnimator.SetBool("WalkingRight", false);
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
        else { Instantiate(hitPartical, transform.position, Quaternion.identity);  }
    }

    private void Death()
    {
        FindObjectOfType<LevelController>().EnemyDeathIndexReset(ESI.LevelIndex, ESI.SpawnIndex);
        FindObjectOfType<AudioManager>().Play("BasicEnemyDeath");
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
