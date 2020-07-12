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
    public int numLevel, numPlanet;

    public void loadGame()
    {
        GameRule.mode = mode;
        GameRule.type = type;
        GameRule.AI = AI;
        GameRule.difficulties = difficulties;
        GameRule.levelNum = numLevel;
        GameRule.planetNum = numPlanet;

        MenuManager.cameraStatus = MenuManager.Status.freeOnMenu;
        SceneManager.LoadScene("Game");
    }
}
