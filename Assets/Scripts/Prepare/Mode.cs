using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Mode
{
    void initScene();
    void calculateResult();
    void gameOver();
    void changeCount(bool dirrection);
}