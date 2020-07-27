using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseStructures;

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
            downCountText.text = (++downCount).ToString();
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
        for (int i = sec; i >= 1; --i)
        {
            gameCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        LocalizationManager.add(new Pair<Text, string>(gameCounterText, "go"));
        capperField.SetActive(false);
        Game.activeGame = true;
        AI.GetComponent<AI>().active = true;    
        yield return new WaitForSeconds(1);
        StartCoroutine(counter(60));
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
