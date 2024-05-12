using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    void OnEnable()
    {
        HighScores.SetHighScores(ScoreScript.GetScore());
    }

}
