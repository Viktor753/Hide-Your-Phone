using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAnimation : MonoBehaviour
{
    public NavMeshAgent agent;
    public Animator animator;


    private void Update()
    {
        animator.SetFloat("vertical", agent.velocity.z);
        animator.SetFloat("horizontal", agent.velocity.x);
    }
}
