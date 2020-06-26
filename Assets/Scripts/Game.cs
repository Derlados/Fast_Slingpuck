using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    /* gameOverText - текст с сообщение об окончании игры
     * upCountText - текст показывающий счет верхнего игрока
     * downCountText - текст показывающий счет нижнего игрока
     * gameStartCounterText - текст показывающий отчет до начала игры
     */
    public Text gameOverText, upCountText, downCountText, gameStartCounterText;
    private bool gameOver, gamePaused;
    public GameObject pauseMenuCanvas, gameOverCanvas;
    public GameObject AI;
    public GameObject capper;

    bool gameStarted;

    private void Start()
    {
        StartCoroutine(delaySec(1));  //отчет
        StartCoroutine(delayAI(3));
    }

    void Update()
    {
        if (gameStarted)
        {
            upCountText.text = Checker.upCount.ToString();
            downCountText.text = Checker.downCount.ToString();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gameOver)
                {
                    if (!gamePaused)
                        PauseMenu();
                    else
                        UnPauseMenu();
                }
            }

            if (!gameOver)
            {
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

    // Задержка перед стартом игры
    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
        for (int i = 2; i > 0; --i)
        {
            gameStartCounterText.text = i.ToString();
            yield return new WaitForSeconds(sec);
        }
        gameStartCounterText.text = "GO!";
        capper.SetActive(false);
        gameStarted = true;

        yield return new WaitForSeconds(sec);
        gameStartCounterText.enabled = false;
    }
    
    // Задержка перед запуском бота
    IEnumerator delayAI(float sec)
    {
        yield return new WaitForSeconds(sec);
        AI.GetComponent<AI>().active = true;
    }


}
