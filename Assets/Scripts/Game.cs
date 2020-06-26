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
     * gameStartCounterText - текст показывающий отсчет до начала игры
     * scoreText - текст показывающий набранные очки в игре
     */
    public Text gameOverText, upCountText, downCountText, gameStartCounterText, scoreText;
    private bool gameOver, gamePaused;
    public GameObject pauseMenuCanvas, gameOverCanvas;
    public GameObject AI;
    public GameObject capper;

    public static int score;
    public static bool gameStarted;

    private void Start()
    {
        StartCoroutine(delaySec(1));  //отсчет
        StartCoroutine(delayAI(3));
    }

    void Update()
    {
        if (gameStarted)
        {
            upCountText.text = Checker.upCount.ToString();
            downCountText.text = Checker.downCount.ToString();

            //вызов паузы
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
                    //включаем экран окончания игры
                    gameOver = true;
                    gameStarted = false;
                    AI.GetComponent<AI>().active = false;
                    gameOverCanvas.SetActive(true);
                    //установка текста 
                    gameOverText.text = "Up Win!";
                    scoreText.text = "YOUR SCORE IS " + score;
                    //сохраняем очки
                    XMLManager.ins.SavePlayer(score);
                    score = 0;
                }

                if (Checker.downCount == 0)
                {
                    //включаем экран окончания игры
                    gameOver = true;
                    gameStarted = false;
                    AI.GetComponent<AI>().active = false;
                    gameOverCanvas.SetActive(true);
                    //установка текста 
                    gameOverText.text = "Down Win!";
                    scoreText.text = "YOUR SCORE IS " + score;
                    //сохраняем очки
                    XMLManager.ins.SavePlayer(score);
                    score = 0;
                }
            }
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
    }
    
    // Задержка перед запуском бота
    IEnumerator delayAI(float sec)
    {
        yield return new WaitForSeconds(sec);
        AI.GetComponent<AI>().active = true;
    }


}
