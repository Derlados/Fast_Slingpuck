using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text scoreText;
    LocalizationManager locManager;
    XMLManager manager;
    GameManager gameManager;

    private void Start()
    {
        manager = XMLManager.getInstance();
        locManager = LocalizationManager.getInstance();
        locManager.Init();
        manager.LoadPlayer();
        scoreText.text = manager.player.score.ToString();
        gameManager = GameManager.getInstance();
    }

    public void PlayPressed()
    {
        gameManager.setNormalMode();
        SceneManager.LoadScene("Game");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

    public void SpeedGamePressed()
    {
        gameManager.setSpeedGame();
        SceneManager.LoadScene("Game");
    }
}
