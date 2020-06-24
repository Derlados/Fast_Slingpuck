using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public Text gameOverText;
    private bool gameOver, gamePaused;
    public GameObject pauseMenuCanvas, gameOverCanvas;
    public GameObject AI;

    private void Start()
    {
        StartCoroutine(delaySec(20));
        Debug.Log("Start");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameOver)
            {
                if (!gamePaused)
                {
                    PauseMenu();
                }
                else
                {
                    UnPauseMenu();
                }
            }
        }

        if (Checker.upCount == 0)
        {
            gameOverText.text = "Up Win!";
            gameOver = true;
            Time.timeScale = 0;
            gameOverCanvas.SetActive(true);
            AI.GetComponent<AI>().active = false;
        }

        if (Checker.downCount == 0)
        {

            gameOverText.text = "Down Win!";
            gameOver = true;
            Time.timeScale = 0;
            gameOverCanvas.SetActive(true);
            AI.GetComponent<AI>().active = false;
        }
        // Debug.Log("upCount=" + Checker.upCount + " and downCount=" + Checker.downCount);
    }

    public void PauseMenu()
    {
        gamePaused = true;
        Time.timeScale = 0;
        pauseMenuCanvas.SetActive(true);
    }

    public void UnPauseMenu()
    {
        gamePaused = false;
        Time.timeScale = 1;
        pauseMenuCanvas.SetActive(false);
    }

    public void ToMainMenuPressed()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
    }
}
