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
    bool gameStarted;

    private void Start()
    {
        StartCoroutine(delaySec(2));
        StartCoroutine(delayAI(1));
    }

    void Update()
    {
        if (gameStarted)
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
                gameOverCanvas.SetActive(true);
                AI.GetComponent<AI>().active = false;
            }

            if (Checker.downCount == 0)
            {

                gameOverText.text = "Down Win!";
                gameOver = true;
                gameOverCanvas.SetActive(true);
                AI.GetComponent<AI>().active = false;
            }
            // Debug.Log("upCount=" + Checker.upCount + " and downCount=" + Checker.downCount);
        }
    }

    public void PauseMenu()
    {
        gamePaused = true;
        Time.timeScale = 0f;
        pauseMenuCanvas.SetActive(true);
    }

    public void UnPauseMenu()
    {
        gamePaused = false;
        Time.timeScale = 1f;
        pauseMenuCanvas.SetActive(false);
    }

    public void ToMainMenuPressed()
    {
        SceneManager.LoadScene("Menu");
        Checker.upCount = 0;
        Checker.downCount = 0;
        Time.timeScale = 1f;
    }

    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
        gameStarted = true;
    }

    IEnumerator delayAI(float sec)
    {
        yield return new WaitForSeconds(sec);
        AI.GetComponent<AI>().active = true;
    }


}
