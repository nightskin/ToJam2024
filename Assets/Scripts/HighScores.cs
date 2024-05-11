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
        if(score > firstPlace) firstPlace = score;
        else if (score > secondPlace) secondPlace = score;
        else if (score > thirdPlace) thirdPlace = score;
    }
}
