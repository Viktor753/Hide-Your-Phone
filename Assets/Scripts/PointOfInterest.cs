using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PointOfInterest : MonoBehaviour
{
    public Transform desiredPosition;
    public float actionDuration = 3.0f;

    public Action<PointOfInterest> OnSequenceBegin;
    public Action<PointOfInterest> OnActionStarted;
    public Action<PointOfInterest> OnActionComplete;

    public void MoveToPoint()
    {
        OnSequenceBegin?.Invoke(this);
        GuardMovement.instance.MoveToLocation(desiredPosition.position, PerformAction);
    }

    private void PerformAction()
    {
        GuardMovement.instance.LookAtTarget(desiredPosition);
        OnActionStarted?.Invoke(this);
        Invoke(nameof(EndAction), actionDuration);
    }

    private void EndAction()
    {
        GuardMovement.instance.LookAtTarget(null);
        OnActionComplete?.Invoke(this);
    }
}
