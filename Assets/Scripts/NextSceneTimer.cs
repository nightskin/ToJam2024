using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneTimer : MonoBehaviour
{
    public float timer = 1.5f;

    void Start()
    {
        
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
