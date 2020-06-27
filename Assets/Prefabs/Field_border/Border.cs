using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    private void Start()
    {
        ScreenOptimization.setColider(gameObject, this.GetComponent<BoxCollider2D>());
    }
}
