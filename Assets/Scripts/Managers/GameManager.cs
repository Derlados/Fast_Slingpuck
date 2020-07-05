using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;


public class GameManager 
{
    public enum modes : byte
    {
        Normal,
        Speed
    }

    public static modes currentMode = modes.Normal;
    public static string level;

    public static void setNormalMode()
    {
        currentMode = modes.Normal;
    }

    public static void setSpeedGame()
    {
        currentMode = modes.Speed;
    }
}
