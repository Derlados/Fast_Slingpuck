using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    public GameObject game;

    void Start()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }
}
