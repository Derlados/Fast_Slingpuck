using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sand : MonoBehaviour, Field
{
    Checker checkerData;

    public void setGlobalModififers(GameObject checkers)
    {
        for (int i = 0; i < checkers.transform.childCount; ++i)
            checkers.transform.GetChild(i).gameObject.AddComponent<Magnet>();
        Magnet.force = 10f;
        StartCoroutine(delay(2));
        checkerData = checkers.transform.GetChild(0).GetComponent<Checker>();
    }

    // Устанавливает случайны образом направление ветра
    void randomDir()
    {
        System.Random rand = new System.Random();
        int value = rand.Next(0, 2);
        if (value == 0)
            Magnet.border = checkerData.playerUpBorder.Right;
        else
            Magnet.border = checkerData.playerUpBorder.Left;
        StartCoroutine(delay(5));
    }

    // Задержка, раз в sec секунд времени будет происходить событие по которому может сменится направление поля, а может остаться 
    IEnumerator delay(float sec)
    {
        yield return new WaitForSeconds(sec);
        randomDir();
    }
}
