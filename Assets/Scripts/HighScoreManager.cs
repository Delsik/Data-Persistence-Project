using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    public TextMeshProUGUI highScoresText;

    void Start()
    {
        DisplayHighScores();
    }

    void DisplayHighScores()
    {
        highScoresText.text = "High Scores:\n";

        int count = PlayerPrefs.GetInt("HighScoreCount", 0);
        for (int i = 0; i < count; i++)
        {
            string name = PlayerPrefs.GetString("HighScoreName" + i);
            int score = PlayerPrefs.GetInt("HighScoreScore" + i);
            highScoresText.text += $"{i + 1}. {name}: {score}\n";
        }
    }
}
