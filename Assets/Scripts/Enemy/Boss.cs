using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool IsChasing;
    [SerializeField] float ChaseDistance;
    [SerializeField] int Health;


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

    public void DecreaseHealth()
    {
        Health--;
        if (Health <= 0)
        { 
            Destroy(gameObject);
        }
    }
}
