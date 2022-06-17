using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    public bool enable = true;

    public float fieldOfViewAngle = 145.0f;
    public float fieldOfViewRange = 10.0f;

    public bool inRange = false;
    public bool inAngle = false;

    private Transform playerTarget;

    private void Start()
    {
        playerTarget = Player.instance.transform;
    }

    private void Update()
    {
        if(enable == false)
        {
            inRange = false;
            inAngle = false;
            return;
        }

        inRange = Vector3.Distance(transform.position, playerTarget.position) < fieldOfViewRange;
        inAngle = Vector3.Angle(playerTarget.position - transform.position, transform.forward) < (fieldOfViewAngle * 0.5f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0) * transform.forward * fieldOfViewRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0) * transform.forward * fieldOfViewRange);
    }
}
