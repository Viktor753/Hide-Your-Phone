using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PointsManager : MonoBehaviour
{
    public bool losePointsOverTime = false;

    public int points;
    public int pointsMultiplier;
    public int maxMultipler = 32;

    private float currentMultiplerTimer = 0.0f;
    public float timeToIncrementMultipler = 3.0f;

    private float currentPointIncrementTimer = 0.0f;
    public float timeBetweenPointsIncrement = 1.0f;

    public float timeBetweenCoinDecrease = 1.0f;
    private float currentLosePointTimer = 0.0f;

    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI multiplierText;
    public Slider multiplerProgressSlider;

    private bool playerObjectIsUp = false;

    private void OnEnable()
    {
        GameManager.OnGameEnded += OnGameEnded;
        GameManager.OnGameReset += ResetPoints;
    }

    private void OnDisable()
    {
        GameManager.OnGameReset -= ResetPoints;
        GameManager.OnGameEnded -= OnGameEnded;
    }

    private void Update()
    {
        if(GameManager.gameStarted == false)
        {
            return;
        }

        playerObjectIsUp = Player.instance.playerObjectUp;

        if (playerObjectIsUp)
        {
            currentMultiplerTimer += Time.deltaTime;
            currentMultiplerTimer = Mathf.Clamp(currentMultiplerTimer, 0, timeToIncrementMultipler);
            if (currentMultiplerTimer >= timeToIncrementMultipler)
            {
                if (pointsMultiplier * 2 <= maxMultipler)
                {
                    ChangeMultipler(pointsMultiplier * 2);
                }
            }

            currentPointIncrementTimer += Time.deltaTime;

            if(currentPointIncrementTimer >= timeBetweenPointsIncrement)
            {
                currentPointIncrementTimer = 0.0f;
                EditPoints(Mathf.FloorToInt(1 * pointsMultiplier));
            }
        }
        else
        {
            currentMultiplerTimer -= Time.deltaTime;
            currentMultiplerTimer = Mathf.Clamp(currentMultiplerTimer, 0, maxMultipler);

            if(pointsMultiplier > 1 && currentMultiplerTimer <= 0)
            {
                ChangeMultipler(pointsMultiplier / 2);
                currentMultiplerTimer = timeToIncrementMultipler - (timeToIncrementMultipler * 0.01f);
            }

            if (losePointsOverTime)
            {
                currentLosePointTimer += Time.deltaTime;
                if(currentLosePointTimer >= timeBetweenCoinDecrease)
                {
                    currentLosePointTimer = 0.0f;
                    EditPoints(-1);
                }
            }
        }

        multiplerProgressSlider.value = currentMultiplerTimer / timeToIncrementMultipler;
    }

    private void EditPoints(int amount)
    {
        if(amount > 0)
        {
            //increment of points
        }else if(amount < 0)
        {
            //loss of points
        }

        points += amount;
        pointsText.text = $"Points {points.ToString()}";
    }

    private void ChangeMultipler(int newMultipler)
    {
        multiplierText.text = $"X{newMultipler.ToString()}";
        pointsMultiplier = newMultipler;
        currentMultiplerTimer = 0.0f;
    }

    public void ResetPoints()
    {
        currentPointIncrementTimer = 0.0f;
        currentMultiplerTimer = 0.0f;
        currentLosePointTimer = 0.0f;

        points = 0;
        pointsMultiplier = 1;

        pointsText.text = $"Points {points}";
        multiplierText.text = $"X{pointsMultiplier}";
        multiplerProgressSlider.value = 0.0f;
    }

    private void OnGameEnded()
    {
        if(points > PlayerData.highscore)
        {
            PlayerData.SetHighscore(points);
        }
    }
}
