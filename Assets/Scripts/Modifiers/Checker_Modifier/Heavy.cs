using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : Modifier
{
    void Start()
    {
        Debug.Log(GetComponent<Checker>().body.mass);
        GetComponent<Checker>().body.mass *= 10;
        Debug.Log(GetComponent<Checker>().body.mass);
        GetComponent<Checker>().coefForce = 10;
    }
}
