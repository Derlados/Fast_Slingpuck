using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseStructures;
using GooglePlayGames;

public class Speed : MonoBehaviour, Mode
{
    public GameObject AI;
    public GameObject capperField;
    public GameObject gameMenu;
    public GameObject downBorderHolder;
    public Game game;

    public Text upCountText, downCountText, gameCounterText;

    // Счетчики
    public int score;
    public short upCount = 0, downCount = 0; // константы (4) необходимо заменить

    // Цели
    public int winTarget; // На случай если нету ИИ, победа начисляется за количество фишек забитых за время
    public int targetCheckers; // Вторая цель - количество забитых фишек, для получения следующей звезды
    public int maxLag = 0; // Максимальное отставание от ИИ которое было за игру
    

    // Монеты
    int money1, money2, money3;

    // Start is called before the first frame update
    void Start()
    {
        initScene();
        StartCoroutine(delayBeforeStart(3));
    }

    public void initScene()
    {
        game = GetComponent<Game>();

        // Текст счетчиков
        upCountText = game.upCountText;
        downCountText = game.downCountText;
        gameCounterText = game.gameCounter;
        upCountText.text = upCount.ToString();

        downBorderHolder = game.downBorderHolder;

        // Бот
        AI = game.AI;
        if (GameRule.TypeAI == GameRule.AI.None)
            upCountText.gameObject.SetActive(false);

        // Установка начального текста для счетчиков
        downCountText.text = downCount.ToString();

        // Меню
        gameMenu = game.gameMenu;

        // Заглушка
        capperField = game.capperField;
        game.checkersSpeed.SetActive(true);

        // Установка целей
        initTargets();
    }


    /* Установка целей для режима Normal
     * Порядок целей для режима (номер цели = номеру цели в GameRule)
     * 1 - Победа (Или достигнута цель winTarget)
     * 2 - Количество фишек для получения следующей звезды
     * 3 - Без промахов (нету ИИ), не отставать от бота в течении всей игры
     */
    public void initTargets()
    {
        winTarget = GameRule.target1;
        targetCheckers = GameRule.target2;
    }

    // Очко добавляется тому игроку с чьей стороны прилетела шайба
    public void changeCount(GameObject obj)
    {
        if (obj.GetComponent<Checker>().field == Checker.Field.Down)
        {
            downCountText.text = (++downCount).ToString();
            PlayGamesPlatform.Instance.IncrementAchievement(GPGSIds.achievement_control_of_universe, 1, null);
            AudioManager.PlaySound(AudioManager.Audio.rise03);
        }  
        else
            upCountText.text = (++upCount).ToString();

        if (upCount - downCount > maxLag)
            maxLag = upCount - downCount;
    }

    public void gameOver()
    {
        calculateResult();
        AI.GetComponent<AI>().active = false;
        gameMenu.GetComponent<GameMenu>().gameOver("Game Over !", game.countStars, money1, money2, money3); 
    }

    public void calculateResult()
    {
        // Подсчет звезд и монет
        int accuracyOrLagMoney = 80, victoryMoney = 15;
        money1 = money2 = money3 = 0;

        double accuracy = ((double)downCount / Game.countShots);

        // Начисление звезд
        if ((GameRule.TypeAI == GameRule.AI.None && downCount < winTarget) || (GameRule.TypeAI != GameRule.AI.None && downCount <= upCount))
            game.countStars = 0;
        else
        {
            if (downCount < targetCheckers)
                --game.countStars;

            if ((GameRule.TypeAI != GameRule.AI.None && maxLag > 0) || (GameRule.TypeAI == GameRule.AI.None && accuracy < GameRule.target3))
                --game.countStars;
        }

        money1 = game.countStars * victoryMoney;

        /* Начисление за точность если игра была без ИИ
         * Формула: Процент точности * accuracyOrLagMoney
         * Начисление монет если игра была против ИИ
         * За каждое очко на которое максимально отстал от ИИ игрок - штраф 1/5 от полного зароботка, таким образом отставание более чем на 5 голов приводит к тому что игрок получает 0 монет
         */
        money2 = GameRule.TypeAI == GameRule.AI.None ? (int)(accuracyOrLagMoney * accuracy) : accuracyOrLagMoney - (maxLag * accuracyOrLagMoney / 5);
        if (money2 < 0 || money1 == 0)
            money2 = 0;

        // За каждый гол - +2 монеты, начисляется даже при поражении 
        money3 = downCount * 2;
    }

    // Задержка перед началом игры
    IEnumerator delayBeforeStart(int sec)
    {
        RectTransform rect = gameCounterText.GetComponent<RectTransform>();
        Vector3 rotPos = rect.rotation.eulerAngles;
        gameCounterText.text = sec.ToString();

        //раскрутка надписей
        for (int i = sec; i >= 1; --i)
        {
            //раскрутка цифр
            if (i != 1)
            {
                rotPos.z = 0;
                //поворот на 360 градусов
                for (float j = 1; j <= 361; j += 30)
                {
                    yield return new WaitForSeconds(0.20f / j);
                    rect.rotation = Quaternion.Euler(rotPos);
                    rotPos.z -= 30;

                }

                //на максимальных оборотах меняем цифру на 1 меньше     
                gameCounterText.text = (i - 1).ToString();
                AudioManager.PlaySound(AudioManager.Audio.count);

                //проделываем обороты до нормальной видимой скорости
                for (float j = 361; j > 1; j -= 30)
                {
                    yield return new WaitForSeconds(0.20f / j);
                    rect.rotation = Quaternion.Euler(rotPos);
                    rotPos.z -= 30;
                }
            }

            //затухание последней цифры 1
            else
            {
                AudioManager.PlaySound(AudioManager.Audio.endCount);
                while (gameCounterText.color.a >= 0)
                {
                    float time = Time.deltaTime / 1;
                    Color color = gameCounterText.color;
                    color.a -= time; ;
                    gameCounterText.color = new Color(color.r, color.g, color.b, color.a);
                    yield return new WaitForSeconds(0.001f);
                }

                capperField.SetActive(false);
                AI.GetComponent<AI>().active = true;
                Game.activeGame = true;
            }
            yield return new WaitForSeconds(1);
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(counter(10));
    }

    // Таймер игры
    IEnumerator counter(int sec)
    {
        for (int i = sec; i >= 0; --i)
        {
            gameCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        AI.GetComponent<AI>().active = false;
        gameOver();
    }

}
