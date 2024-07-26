using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool IsChasing;
    [SerializeField] float ChaseDistance;
    public EnemySpawnInformation ESI;
    [SerializeField] Rigidbody2D RB;
    [SerializeField] float PushbackForce;

    void Start()
    {
        if (TrackingTarget == null)
        {
            TrackingTarget = FindObjectOfType<PlayerScript>().gameObject.transform;
        }
    }
    void Update()
    {
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
    }

    public void DecreaseHealth(float damageDealt)
    {
        ESI.Health -= damageDealt;
        if (ESI.Health <= 0)
        {
            Death();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
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
        Destroy(gameObject);
    }
}
