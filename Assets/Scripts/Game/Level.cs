using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    public GameRule.Mode mode;
    public GameRule.Type type;

    public void loadGame()
    {
        GameRule.mode = mode;
        GameRule.type = type;
        SceneManager.LoadScene("Game");
    }
}
