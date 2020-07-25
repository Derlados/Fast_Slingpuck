using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    void Awake()
    {
        ScreenOptimization.setColider(gameObject, gameObject.GetComponent<BoxCollider2D>());
    }
}
