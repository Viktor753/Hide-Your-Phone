using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static int highscore { get; private set; }

    public static void SetHighscore(int value)
    {
        PlayerPrefs.SetInt("highscore", value);
        highscore = value;
    }

    public static int LoadHighscore()
    {
        highscore = PlayerPrefs.GetInt("highscore");
        return highscore;
    }
}
