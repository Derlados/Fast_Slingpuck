using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    Text text;

    private void Start()
    {
        text = GetComponent<Text>();
        text.enabled = false;
    }

    void Update()
    {
        if (Checker.upCount == 0)
        {
            text.text = "Up Win!";
            text.enabled = true;
            //Debug.Log("Up Win!");
           // Debug.Log("upCount=" + Checker.upCount + " and downCount=" + Checker.downCount);

        }

        if (Checker.downCount == 0)
        {
            text.text = "Down Win!";
            text.enabled = true;
           // Debug.Log("down Win!");
           // Debug.Log("upCount=" + Checker.upCount + " and downCount=" + Checker.downCount);
        }
    }
}
