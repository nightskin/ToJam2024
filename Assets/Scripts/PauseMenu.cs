using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private void OnEnable() 
    {
        GameObject.Find("EventSystem").GetComponent<EventSystem>().SetSelectedGameObject(transform.Find("Resume").gameObject);
    }
}
