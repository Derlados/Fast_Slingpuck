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

    private void Start()
    {
        manager = XMLManager.getInstance();
        manager.LoadPlayer();
        scoreText.text = manager.player.score.ToString();
    }

    public void PlayPressed()
    {
        LocalizationManager.instance.clear();
        SceneManager.LoadScene("Game");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }
}
