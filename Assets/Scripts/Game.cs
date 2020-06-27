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
    public static byte upCount = 0, downCount = 0;

    XMLManager manager;
    GameManager gameManager;
    private void Start()
    {
        manager = XMLManager.getInstance();
        gameManager = GameManager.getInstance();
        Debug.Log(gameManager.currentMode);

        if (gameManager.currentMode == GameManager.modes.Normal)
        {
            StartCoroutine(delaySec(1));  //отсчет
            StartCoroutine(delayAI(3));
        }
        else
        {
            AI.GetComponent<AI>().active = false;
            StartCoroutine(countDownTimer(60));
        }

    }

    void Update()
    {
        if (gameStarted)
        {
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

            if (gameManager.currentMode == GameManager.modes.Normal)
            {
                upCountText.text = upCount.ToString();
                downCountText.text = downCount.ToString();

                if (!gameOver)
                {
                    if (upCount == 0)
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
                        manager.SavePlayer(score);
                        score = 0;
                    }

                    if (downCount == 0)
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
                        manager.SavePlayer(score);
                        score = 0;
                    }
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
        SceneManager.LoadScene("MainMenu");
        upCount = 0;
        downCount = 0;
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

    //время до конца игры в режиме на скорость
    IEnumerator countDownTimer(int time)
    {
        for (int i = time; i > 0; --i)
        {
            gameStartCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        //включаем экран окончания игры
        gameOver = true;
        gameStarted = false;
        gameOverCanvas.SetActive(true);
        //установка текста 
        gameOverText.text = "Time ended!";
        scoreText.text = "YOUR SCORE IS " + score;
        //сохраняем очки
        manager.SavePlayer(score);
        score = 0;
    }

    static public void IncreaseCount(bool up)
    {
        if (up)
        {
            upCount++;
            if (gameStarted)
                score += 100;
        }
        else
            downCount++;
    }

    static public void DecreaseCount(bool up)
    {
        if (up)
            upCount--;
        else
            downCount--;
    }
}
