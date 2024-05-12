using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public static Animator animator;
    public static float transitionTime = 1;
    bool startSceneTransition = false;
    void Start()
    {
        animator = GetComponent<Animator>();
    }


    public static IEnumerator LoadScene(string name)
    {
        animator.SetTrigger("fade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(name);
    }

    public static IEnumerator LoadScene(int index)
    {
        animator.SetTrigger("fade");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(index);
    }


}
