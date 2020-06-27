using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

/* Класс для работы режимами игры
 * Singleton
 */
public class GameManager 
{
    public static GameManager instance = null;

    public enum modes : byte
    {
        Normal,
        Speed
    }

    public modes currentMode = modes.Normal;

    private GameManager()
    { }
    public static GameManager getInstance()
    {
        if (instance == null)
            instance = new GameManager();
        return instance;
    }


    public void setNormalMode()
    {
        currentMode = modes.Normal;
    }

    public void setSpeedGame()
    {
        currentMode = modes.Speed;
    }
}
