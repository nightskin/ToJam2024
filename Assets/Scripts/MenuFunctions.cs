using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    public void HighScores()
    {
        StartCoroutine(LevelLoader.LoadScene("Scores"));
    }
    
    public void MainMenu()
    {
        StartCoroutine(LevelLoader.LoadScene("Menu"));
        Time.timeScale = 1;
    }

    public void PlayGame()
    {
        StartCoroutine(LevelLoader.LoadScene("Game"));
    }

    public void Retry()
    {
        StartCoroutine( LevelLoader.LoadScene(SceneManager.GetActiveScene().name));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
