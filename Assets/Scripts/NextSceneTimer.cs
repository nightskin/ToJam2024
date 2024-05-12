using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTimer : MonoBehaviour
{
    public float timer = 1.5f;


    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            StartCoroutine(LevelLoader.LoadScene(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }
}
