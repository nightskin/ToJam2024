using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    static int score;
    static TextMeshProUGUI scoreText;

    void Start()
    {
        score = 0;
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    public static void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "SCORE:" + score.ToString();
    }
}
