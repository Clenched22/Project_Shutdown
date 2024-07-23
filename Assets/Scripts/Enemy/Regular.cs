using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class Regular : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;
    [SerializeField] float ChaseDistance;
    [SerializeField] float MoveSpeed;
    [SerializeField] bool IsChasing;


    void Start()
    {
        if (TrackingTarget == null)
        {
            TrackingTarget = FindObjectOfType<PlayerScript>().gameObject.transform;
        }
    }
    void Update()
    {

        if (TrackingTarget != null)
        {

        }
    }

    void FixedUpdate()
    {

    }
}
