using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameOverMenu : MonoBehaviour
{
    void OnEnable()
    {
        HighScores.SetHighScores(ScoreScript.GetScore());
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(transform.Find("Retry").gameObject);
    }

}
