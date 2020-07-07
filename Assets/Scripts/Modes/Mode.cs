using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Mode
{
    void initScene();
    void calculateResult();
    void gameOver();

    /* Функция счета очков при удачном попадании в "окно"
    * Параметры:
    * direction - направление с которого вышла шайба
    * true - снизу, false - сверху
    */
    void changeCount(bool dirrection);
}