using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Класс отвечающий за весь UI в самой игре
public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas, gameOverCanvas, capperField, PauseBtnCanvas;
    public Text gameOverText, scoreText;
    PlayerData playerData;

    public void Start()
    {
        playerData = PlayerData.getInstance();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        capperField.SetActive(true);
        pauseMenuCanvas.SetActive(true);
        PauseBtnCanvas.SetActive(false);
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        capperField.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        PauseBtnCanvas.SetActive(true);
    }
    public void ToMainMenuPressed()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameMenu");
    }

    public void gameOver(string message, int money)
    {
        gameOverCanvas.SetActive(true);
        capperField.SetActive(true);
        scoreText.text = money.ToString();
        PlayerData.getInstance().money += money;
        XMLManager.SaveData(PlayerData.getInstance(), PlayerData.getInstance().ToString());

        gameOverText.text = message;
    }

}
