using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderHolder : MonoBehaviour
{
    void Start()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }
}
