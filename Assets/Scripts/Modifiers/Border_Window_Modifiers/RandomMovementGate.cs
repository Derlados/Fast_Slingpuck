using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementGate : MovementGate
{
    void Start()
    {
        base.Start();
        StartCoroutine(delay(3));
    }

    void randomDir()
    {
        stepWorld = -stepWorld;
        stepScreen = -stepScreen;

        StartCoroutine(delay(Random.Range(0, 3f)));
    }

    IEnumerator delay(float sec)
    {
        yield return new WaitForSeconds(sec);
        randomDir();
    }
}
