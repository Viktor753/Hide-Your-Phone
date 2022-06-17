using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;

public class GuardMovement : MonoBehaviour
{
    public Transform guardParent;
    public float stoppingDistanceFromPlayer = 2.5f;
    public float defaultStoppingDistance = 0;
    public float lookAtSpeed = 1.0f;

    public bool doingAction = false;

    public static GuardMovement instance;
    public NavMeshAgent agent;

    private Coroutine moveToLocationCoroutine;
    private Coroutine lookAtTargetCoroutine;

    private void Awake()
    {
        instance = this;
        agent.stoppingDistance = defaultStoppingDistance;
    }


    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameEnded -= OnGameEnded;
    }

    public void MoveToLocation(Vector3 position, Action OnArrived)
    {
        if(moveToLocationCoroutine != null)
        {
            StopCoroutine(moveToLocationCoroutine);
        }
        moveToLocationCoroutine = StartCoroutine(MoveToLocationEnumerator(position, OnArrived, 0.2f));
    }

    private IEnumerator MoveToLocationEnumerator(Vector3 position, Action OnArrived, float arrivedOffset)
    {
        agent.SetDestination(position);

        //agent.remainingDistance might not be set, wait a little bit before continuing
        yield return null;

        while(agent.remainingDistance > 0)
        {
            if(agent.remainingDistance < arrivedOffset)
            {
                OnArrived?.Invoke();
                break;
            }
            yield return null;
        }
    }

    private void OnGameStarted()
    {
        agent.stoppingDistance = defaultStoppingDistance;
    }

    private void OnGameEnded()
    {
        if (moveToLocationCoroutine != null)
        {
            StopCoroutine(moveToLocationCoroutine);
        }

        Debug.Log("Reseting guard...");
        var playerPos = Player.instance.playerCamera.transform.position;
        var dstToPlayer = Vector3.Distance(transform.position, playerPos);

        if(dstToPlayer < stoppingDistanceFromPlayer)
        {
            agent.stoppingDistance = dstToPlayer;
        }
        else
        {
            agent.stoppingDistance = stoppingDistanceFromPlayer;
        }

        agent.SetDestination(Player.instance.playerCamera.transform.position);
    }

    public void LookAtTarget(Transform target)
    {
        if (lookAtTargetCoroutine != null)
        {
            StopCoroutine(lookAtTargetCoroutine);
        }


        if (target == null)
        {
            lookAtTargetCoroutine = StartCoroutine(SmoothLookAtTarget(Quaternion.Euler(0,0,0), true));
        }
        else
        {
            lookAtTargetCoroutine = StartCoroutine(SmoothLookAtTarget(Quaternion.LookRotation(target.forward), false));
        }
    }

    private IEnumerator SmoothLookAtTarget(Quaternion newRotation, bool local)
    {
        if (local)
        {
            while (transform.localRotation != newRotation)
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, newRotation, Time.deltaTime * lookAtSpeed);
                yield return null;
            }
        }
        else
        {
            while (transform.rotation != newRotation)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * lookAtSpeed);
                yield return null;
            }
        }
    }
}
