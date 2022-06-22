using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnvironment : MonoBehaviour
{
    public PointOfInterest[] pointsOfIntrest;

    private PointOfInterest currentInterest;

    private void OnEnable()
    {
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameEnded += OnGameEnded;

        foreach (var poi in pointsOfIntrest)
        {
            poi.OnActionComplete += NextPOI;
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameEnded -= OnGameEnded;

        foreach (var poi in pointsOfIntrest)
        {
            poi.OnActionComplete -= NextPOI;
        }
    }


    private void Start()
    {
        OnGameStarted();
    }

    private void OnGameStarted()
    {
        var prevInterest = currentInterest == null ? null : currentInterest;
        NextPOI(prevInterest);
    }

    private void OnGameEnded()
    {
        currentInterest = null;
    }

    private PointOfInterest GetRandomPOI()
    {
        return pointsOfIntrest[Random.Range(0, pointsOfIntrest.Length)];
    }

    public void NextPOI(PointOfInterest previousInterest)
    {
        Debug.Log("Next POI!");
        for (int i = 0; i < 5; i++)
        {
            currentInterest = GetRandomPOI();
            
            if(currentInterest == previousInterest)
            {
                continue;
            }
            else
            {
                break;
            }
        }

        Debug.Log("New interest: " + currentInterest.name);

        if (GameManager.gameStarted)
        {
            currentInterest.MoveToPoint();
        }
    }
}
