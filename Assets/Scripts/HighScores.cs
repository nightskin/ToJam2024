using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HighScores : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textBox;
    static int firstPlace = 0;
    static int secondPlace = 0;
    static int thirdPlace = 0;


    void Start()
    {
        firstPlace = PlayerPrefs.GetInt("first");
        secondPlace = PlayerPrefs.GetInt("second");
        thirdPlace = PlayerPrefs.GetInt("third");

        if(textBox)
        {
            textBox.text += "1. " + firstPlace.ToString();
            textBox.text += System.Environment.NewLine;
            textBox.text += "2. " + secondPlace.ToString();
            textBox.text += System.Environment.NewLine;
            textBox.text += "3. " + thirdPlace.ToString();
        }
    }

    public static void SetHighScores(int score)
    {
        if (score > firstPlace) 
        { 
            PlayerPrefs.SetInt("first", score);
        }
        else if (score > secondPlace)
        { 
            PlayerPrefs.SetInt("second", score);
        }
        else if (score > thirdPlace) 
        { 
            PlayerPrefs.SetInt("third", score);
        }
    }
}
