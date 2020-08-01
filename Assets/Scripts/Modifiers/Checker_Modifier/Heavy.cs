using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Modifier
{
    void Start()
    {
        GetComponent<Checker>().body.mass *= 10;
        GetComponent<Checker>().coefForce = 10;
    }
}
