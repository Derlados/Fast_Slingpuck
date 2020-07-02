using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text scoreText;

    public void SpeedGamePressed()
    {
        GameManager.setSpeedGame();
        SceneManager.LoadScene("Game");
    }

    public void PlayPressed()
    {
        GameManager.setNormalMode();
        SceneManager.LoadScene("Game");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }


}
