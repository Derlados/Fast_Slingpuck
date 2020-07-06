using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using BaseStructures;
using System.IO;
using System.Runtime.CompilerServices;

// Класс отвечающий за геймплей в самой игре
public class Game : MonoBehaviour
{
    /* gameOverText - текст с сообщение об окончании игры
     * upCountText - текст показывающий счет верхнего игрока
     * downCountText - текст показывающий счет нижнего игрока
     * gameStartCounterText - текст показывающий отсчет до начала игры
     * scoreText - текст показывающий набранные очки в игре
     */
    public GameObject AI;
    public GameObject capperField;
    public GameObject speedGameChecker;
    public GameObject downBorderHolder;
    public GameObject checkers;
    public GameObject gameMenu;
    public GameObject particles;    

    // Счетчики
    public static int score;
    public static byte upCount = 4, downCount = 4; // константы (4) необходимо заменить

    // Текст счетчиков
    public Text upCountText, downCountText, gameCounter;
    //картинка фишки из быстрого режима
    Image image;
    public Image imgField;

    GameRule.Mode mode;
    GameRule.Type type;

    private void Start()
    {
        upCountText.text = upCount.ToString();
        downCountText.text = downCount.ToString();

        mode = GameRule.mode;
        type = GameRule.type;

        if (mode == GameRule.Mode.normal)
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

        ChangePlanetSprite(type.ToString() + "_planet");
        ChangeCheckerSprite(type.ToString() + "_CheckerGlowMat");
        ChangeParticle(type.ToString() + "_particle");
    }

    // Задержка перед стартом игры
    IEnumerator delayBeforeStart(int sec)
    {
        for (int i = sec; i >= 1; --i)
        {
            gameCounter.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        gameCounter.text = "GO!"; // Заменить и локализовать
        capperField.SetActive(false);
        AI.GetComponent<AI>().active = true;
        yield return new WaitForSeconds(1);

        //В режиме Normal текст отсчета выключается
        //В режиме Speed запускается отчет 60 секунд
        if (GameManager.currentMode == GameManager.modes.Normal)
            gameCounter.enabled = false;
        else 
            StartCoroutine(countDownTimer(60));

    }

    // Задержка перед запуском бота
    IEnumerator delayAI(float sec)
    {
        yield return new WaitForSeconds(sec);
        AI.GetComponent<AI>().active = true;
    }

    /* Функция счета очков при удачном попадании в "окно"
     * Параметры:
     * direction - направление с которого вышла шайба
     * true - снизу, false - сверху
     */
    public void changeCount(bool direction)
    {
        if (mode == GameRule.Mode.normal)
        {
            if (direction)
            {
                --downCount;
                ++upCount;
            }
            else
            {
                ++downCount;
                --upCount;
            }

            upCountText.text = upCount.ToString();
            downCountText.text = downCount.ToString();

            if (upCount == 0 || downCount == 0)
                gameOver();
        }
    }

    // Окончание игры
    private void gameOver()
    {
        AI.GetComponent<AI>().active = false;
        gameMenu.GetComponent<GameMenu>().gameOver(0, downCount == 0 ? true : false);   
    }

    // Установка спрайтов поля и шайб
    void ChangePlanetSprite(string spriteName)
    {
        imgField.sprite = Resources.Load<Sprite>("Sprites/levels/planets/" + spriteName);
    }

    void ChangeCheckerSprite(string matName)
    {
        for (int i = 4; i <= 7; ++i)
        {
            Image img = checkers.transform.GetChild(i).gameObject.GetComponent<Image>();
            img.material = Resources.Load<Material>("Sprites/Materials/Checker/" + matName);

            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            Debug.Log(Resources.Load<Material>("Sprites/Materials/Checker/" + matName).color);

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = img.material.GetColor("Color_35045387");
            colorKey[0].time = 0.0f;
            colorKey[1].color = img.material.GetColor("Color_35045387");
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

    void ChangeParticle(string particleName)
    {
        foreach(Transform t in particles.transform)
        {
            if (t.name == particleName)
                t.gameObject.SetActive(true);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////  SPEED Режим  //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    // Анимация уничтожения шайбы
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

    //время до конца игры в режиме на скорость
    IEnumerator countDownTimer(int time)
    {
        for (int i = time; i > 0; --i)
        {
            gameCounter.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        gameOver();
    } 

    //установка шайбы в рандомное место в нижнем поле
    void RandomPosition()
    {
        Pair<Vector2, Vector2> points;
        points = ScreenOptimization.GetWorldCoord2D(downBorderHolder);
        Vector2 randomPos = new Vector2(UnityEngine.Random.Range(points.first.x, points.second.x), UnityEngine.Random.Range(points.first.y, points.second.y));
        speedGameChecker.transform.position = randomPos;
    }

    
}
