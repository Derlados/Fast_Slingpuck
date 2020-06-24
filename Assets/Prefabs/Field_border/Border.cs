using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    ScreenOptimization screenOpt;

    void Start()
    {
        screenOpt = ScreenOptimization.getInstance();
        screenOpt.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }
}
