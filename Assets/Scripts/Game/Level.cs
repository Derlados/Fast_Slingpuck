using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameRule.Mode mode;
    public GameRule.Type type;
    public bool AI;
    public GameRule.Difficulties difficulties;
    public int numLevel;

    public void loadGame()
    {
        GameRule.mode = mode;
        GameRule.type = type;
        GameRule.AI = AI;
        GameRule.difficulties = difficulties;
        GameRule.levelNum = numLevel;
        SceneManager.LoadScene("Game");
    }
}
