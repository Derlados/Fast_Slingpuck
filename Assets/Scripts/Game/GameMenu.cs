using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Класс отвечающий за весь UI в самой игре
public class GameMenu : MonoBehaviour
{
    public Text gameOverText, scoreText;
    public GameObject pauseMenuCanvas, gameOverCanvas, capperField;
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
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        capperField.SetActive(false);
        pauseMenuCanvas.SetActive(false);
    }
    public void ToMainMenuPressed()
    {
        SceneManager.LoadScene("GameMenu");
    }

    public void gameOver(int money, bool winner)
    {
        gameOverCanvas.SetActive(true);
        scoreText.text += money.ToString();
        playerData.money += money;
        XMLManager.SaveData(playerData, playerData.ToString());

        // Временно, текст необходимо будет заменить и локализовать
        if (winner)
            gameOverText.text = "YOU WIN !";
        else
            gameOverText.text = "YOU LOSE !";

    }
}
