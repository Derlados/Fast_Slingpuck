using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ice : MonoBehaviour, Field
{
    // Снижает сопротивление тел в 2 раза
    public void setGlobalModififers(GameObject checkers)
    {
        for (int i = 0; i < checkers.transform.childCount; ++i)
                checkers.transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().drag /= 2;
    }

    public Vector2 correctionForAI(Vector2 aimTarget)
    {
        return aimTarget;
    }
}
