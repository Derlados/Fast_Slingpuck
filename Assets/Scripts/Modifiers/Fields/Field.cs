using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Field
{
    // Устанавливает глобальные модификаторы, влияние самого поля на все шайбы в игре
   void setGlobalModififers(GameObject checkers);
}