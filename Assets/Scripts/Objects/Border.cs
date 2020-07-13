using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    void Start()
    {
        ScreenOptimization.setColider(gameObject, gameObject.GetComponent<BoxCollider2D>());
    }
}
