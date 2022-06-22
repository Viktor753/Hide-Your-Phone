using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static bool gameStarted = false;
    public static GameManager instance;

    public static Action OnGameStarted;
    public static Action OnGameEnded;
    public static Action OnGameReset;

    public PointsManager pointsManager;
    public static Action OnPlayerCaught;
    public Guard guard;

    private void Awake()
    {
        instance = this;
    }

    public void StartGame()
    {
        gameStarted = true;
        OnGameReset?.Invoke();
        OnGameStarted?.Invoke();
    }

    public void StopGame()
    {
        gameStarted = false;
        OnGameReset?.Invoke();
    }

    public void EndGame()
    {
        gameStarted = false;
        OnGameEnded?.Invoke();
        OnGameReset?.Invoke();
    }


    private void Update()
    {
        if (gameStarted == false)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartGame();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                OnGameReset?.Invoke();
            }


            if(guard.playerCanGetCaught && Player.instance.playerObjectUp)
            {
                EndGame();
            }
        }
    }
}
