using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuardUI : MonoBehaviour
{
    public Guard guard;

    private Transform playerTarget;
    public Transform canvas;
    public Image fovImage;

    private void Start()
    {
        playerTarget = Player.instance.transform;
    }

    private void LateUpdate()
    {
        if(guard.inAngle && guard.inRange)
        {
            //Player is seen
            //If player object is up, get caught
            fovImage.color = Color.red;
        }
        else if((guard.inRange == true && guard.inAngle == false) 
            || guard.inRange == false && guard.inAngle == false)
        {
            //Either in range or in FOV

            //Danger
            fovImage.color = Color.yellow;
        }
        else
        {
            //Not in range or FOV

            //No danger at all
            fovImage.color = Color.green;
        }

        canvas.LookAt(playerTarget);
    }
}
