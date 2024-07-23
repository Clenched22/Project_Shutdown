using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform TrackingTarget;

    void Start()
    {
        if (TrackingTarget == null)
        {
            TrackingTarget = FindObjectOfType<PlayerScript>().gameObject.transform;
        }
    }
    private void FixedUpdate()
    {
        FollowTarget();
    }

    private void FollowTarget()
    {
        Vector2 vectorToTarget = TrackingTarget.position - transform.position;
        transform.position = new Vector3(TrackingTarget.position.x, TrackingTarget.position.y, -10);
    }
}
