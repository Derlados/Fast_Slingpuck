using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slip : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().drag /= 2;
    }
}
