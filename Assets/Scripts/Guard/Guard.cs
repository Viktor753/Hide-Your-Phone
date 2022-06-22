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
    public bool playerCanGetCaught = false;

    private Transform playerTarget;

    public float aggroValue;
    private float currentAggroValue;

    private void Start()
    {
        playerTarget = Player.instance.transform;
    }

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStart;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStart;
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


        //If inRange && inAngle && player is holding object up, increase aggro value
        currentAggroValue += (inRange && inAngle && Player.instance.playerObjectUp) ? Time.deltaTime : -Time.deltaTime;
        currentAggroValue = Mathf.Clamp(currentAggroValue, 0, aggroValue);

        //Player caught if aggro value is high enough while in range && angle
        playerCanGetCaught = (inRange && inAngle) && currentAggroValue >= aggroValue ? true : false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, fieldOfViewAngle * 0.5f, 0) * transform.forward * fieldOfViewRange);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -fieldOfViewAngle * 0.5f, 0) * transform.forward * fieldOfViewRange);
    }

    private void OnGameStart()
    {
        currentAggroValue = 0;
    }
}
