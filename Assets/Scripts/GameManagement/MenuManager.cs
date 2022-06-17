using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject GamePanel;

    public TextMeshProUGUI caughtText;
    public TextMeshProUGUI highscoreText;

    private void Awake()
    {
        highscoreText.text = "Highscore " + PlayerData.LoadHighscore().ToString();
    }

    private void OnEnable()
    {
        GameManager.OnGameReset += OnGameStopped;
        GameManager.OnGameStarted += OnGameStarted;
        GameManager.OnGameEnded += OnGameEnded;
    }

    private void OnDisable()
    {
        GameManager.OnGameReset -= OnGameStopped;
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameEnded -= OnGameEnded;
    }

    public void StartPlayButton()
    {
        GameManager.instance.StartGame();
    }

    public void StopPlayButton()
    {
        GameManager.instance.StopGame();
    }

    private void OnGameStarted()
    {
        GamePanel.SetActive(true);
        MenuPanel.SetActive(false);
        caughtText.gameObject.SetActive(false);
    }

    private void OnGameStopped()
    {
        MenuPanel.SetActive(true);
        GamePanel.SetActive(false);
        highscoreText.text = "Highscore " + PlayerData.highscore.ToString();
    }

    private void OnGameEnded()
    {
        caughtText.gameObject.SetActive(true);
    }
}
