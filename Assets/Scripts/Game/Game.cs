using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using BaseStructures;
using System.IO;

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
    public GameObject speedGameChecker;
    public GameObject downBorderHolder;
    public GameObject checkers;

    public static int score;
    public static bool gameStarted;
    public static byte upCount = 0, downCount = 0;

    //картинка фишки из быстрого режима
    Image image;

    PlayerData playerData;

    private void Start()
    {
        playerData = PlayerData.getInstance();

        if (GameManager.currentMode == GameManager.modes.Normal)
        {
            speedGameChecker.SetActive(false);
            StartCoroutine(delayBeforeStart(3));
            StartCoroutine(delayAI(3));
        }
        else
        {
            checkers.SetActive(false);
            StartCoroutine(delayBeforeStart(3));
            AI.GetComponent<AI>().active = false;

        }
        switch (GameManager.level)
        {
            case "lava":
                ChangePlanetSprite("lava_planet");
                ChangeCheckerSprite("fire_CheckerGlowMat", Color.red);
                break;
            case "ice":
                ChangePlanetSprite("ice_planet");
                ChangeCheckerSprite("ice_CheckerGlowMat", Color.blue);
                break;
            case "sand":
                ChangePlanetSprite("sand_planet");
                ChangeCheckerSprite("sand_CheckerGlowMat", Color.yellow);
                break;
            case "jungle":
                ChangePlanetSprite("jungle_planet");
                ChangeCheckerSprite("jungle_CheckerGlowMat", Color.green);
                break;
        }
    }

    void Update()
    {
        if (gameStarted)
        {
            if (GameManager.currentMode == GameManager.modes.Normal)
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
                        playerData.money += score;
                        XMLManager.SaveData(playerData, playerData.ToString());
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
                        playerData.money += score;
                        XMLManager.SaveData(playerData, playerData.ToString());
                        score = 0;
                    }
                }
            }
        }
    }


    // Задержка перед стартом игры
    IEnumerator delayBeforeStart(int sec)
    {
        for (int i = sec; i >= 1; --i)
        {
            gameStartCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        gameStartCounterText.text = "GO!";
        capper.SetActive(false);
        gameStarted = true;
        yield return new WaitForSeconds(1);

        //В режиме Normal текст отсчета выключается
        //В режиме Speed запускается отчет 60 секунд
        if (GameManager.currentMode == GameManager.modes.Normal) gameStartCounterText.enabled = false;
        else StartCoroutine(countDownTimer(60));

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
        XMLManager.SaveData(playerData, playerData.ToString());
        score = 0;
    }

    public void IncreaseCount(bool up)
    {
        if (up)
        {
            upCount++;
            if (gameStarted)
            {
                score += 100;
                if (GameManager.currentMode == GameManager.modes.Speed)
                    StartCoroutine(delayBeforeDissolve());
            }

        }
        else
            downCount++;
    }

    public void DecreaseCount(bool up)
    {
        if (up)
            upCount--;
        else
            downCount--;
    }

    //уничтожение шайбы
    IEnumerator delayBeforeDissolve()
    {
        image = speedGameChecker.GetComponent<Image>();
        for (float f = 0.8f; f >= 0; f -= 0.01f)
        {
            image.material.SetFloat("_DissolveAmount", f);
            yield return new WaitForSeconds(0.01f);
        }
        image.material.SetFloat("_DissolveAmount", 1f);
        RandomPosition();
    }

    //установка шайбы в рандомное место в нижнем поле
    void RandomPosition()
    {
        Pair<Vector2, Vector2> points;
        points = ScreenOptimization.GetWorldCoord2D(downBorderHolder);
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(points.first.x, points.second.x), UnityEngine.Random.Range(points.first.y, points.second.y));
        speedGameChecker.transform.position = randomPos;
    }

    void ChangePlanetSprite(string spriteName)
    {
        Image img = GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>("Sprites/levels/planets/" + spriteName);
    }

    void ChangeCheckerSprite(string matName,Color color)
    {
        for (int i = 4; i <= 7; ++i)
        {
            Image img = checkers.transform.GetChild(i).gameObject.GetComponent<Image>();
            img.material = Resources.Load<Material>("Sprites/Materials/Checker/" + matName);

            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = color;
            colorKey[0].time = 0.0f;
            colorKey[1].color = color;
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.2f;
            alphaKey[1].time = 1.0f;

            gradient = new Gradient();
            gradient.SetKeys(colorKey, alphaKey);

            TrailRenderer trailRenderer = checkers.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
            trailRenderer.colorGradient = gradient;
        }
    }

}
