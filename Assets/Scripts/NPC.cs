using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public Animator animator;
    public float minimumSecondsBetweenNewAnims;
    public float maximumSecondsBetweenNewAnims;

    public string[] animTriggers;

    private float timer;
    private float current_timer;

    private void Update()
    {
        current_timer += Time.deltaTime;

        if(current_timer >= timer)
        {
            current_timer = 0;
            SetNewAnim();
        }
    }

    private void SetNewAnim()
    {
        timer = Random.Range(minimumSecondsBetweenNewAnims, maximumSecondsBetweenNewAnims);

        animator.SetTrigger(animTriggers[Random.Range(0, animTriggers.Length)]);
    }

}
