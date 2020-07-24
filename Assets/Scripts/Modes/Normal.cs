using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Normal : MonoBehaviour, Mode
{
    GameObject AI;
    GameObject capperField;
    GameObject gameMenu;
    Game game;

    // Условия получения звезд
    private int targetTime, targetCheckers; // Константы необходимо передавать из вне
    private int time; // Время игры
    private bool checkTargetCheckers; // Показывает было ли нарушено условие true - нарушено, false - не нарушено

    private int goals = 0; // количество забиваний шайб за всю игру

    // Счетчики
    public int score;
    public byte upCount = 4, downCount = 4; // константы (4) необходимо заменить

    // Текст счетчиков
    public Text upCountText, downCountText, gameCounterText;

    // Монеты
    int money1, money2, money3;
   
    void Start()
    {
        initScene();
        StartCoroutine(delayBeforeStart(3));
    }

    public void initScene()
    {
        game = GetComponent<Game>();

        // Бот
        AI = game.AI;

        // Текст счетчиков
        upCountText = game.upCountText;
        downCountText = game.downCountText;
        gameCounterText = game.gameCounter;

        // Установка начального текста для счетчиков
        upCountText.text = upCount.ToString();
        downCountText.text = downCount.ToString();

        // Меню
        gameMenu = game.gameMenu;

        // Заглушка
        capperField = game.capperField;
        game.checkersNormal.SetActive(true);

        // Установка целей
        initTargets();
    }

    /* Установка целей для режима Normal
     * Порядок целей для режима (номер цели = номеру цели в GameRule)
     * 1 - Победа
     * 2 - Ограничение по времени 
     * 3 - Ограничение по количетву шайб которые могут одновременно находится у игрока 
     */
    public void initTargets()
    {
        targetTime = GameRule.target2;
        targetCheckers = GameRule.target3;
    }

    // Задержка перед стартом игры
    IEnumerator delayBeforeStart(int sec)
    {
        for (int i = sec; i >= 1; --i)
        {
            gameCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        LocalizationManager.add(new Pair<Text, string>(gameCounterText, "go"));
        capperField.SetActive(false);
        AI.GetComponent<AI>().active = true;
        game.activeGame = true;
        yield return new WaitForSeconds(1);
        StartCoroutine(Timer());
    }


    /* Функция счета очков при удачном попадании в "окно"
    * Параметры:
    * obj - шайба которая пролетела через окно
    */
    public void changeCount(GameObject obj)
    {
        ++goals;

        if (obj.transform.position.y > 0)
        {
            --downCount;
            ++upCount;
            AudioManager.PlaySound(AudioManager.Audio.rise03);
        }
        else
        {
            ++downCount;
            --upCount;
        }

        upCountText.text = upCount.ToString();
        downCountText.text = downCount.ToString();

        if (downCount > targetCheckers)
            checkTargetCheckers = true;

        if (upCount == 0 || downCount == 0)
            gameOver();

    }

    // Окончание игры
    public void gameOver()
    {
        StopCoroutine(Timer());
        calculateResult();
        AI.GetComponent<AI>().active = false;
        gameMenu.GetComponent<GameMenu>().gameOver(downCount == 0 ? "YOU WIN !" :  "YOU LOSE !", game.countStars, money1, money2, money3);
        game.ChangeParticle(GameRule.type.ToString() + "_particle", false);
    }

    public void calculateResult()
    {
        PlayerData playerData = PlayerData.getInstance();

        // Подсчет звезд
        if (downCount != 0)
            game.countStars = 0;
        else
        {
            if (checkTargetCheckers)
                --game.countStars;

            if (time > targetTime)
                --game.countStars;
        }

        int timeMoney = 30, goalsMoney = 60, victoryMoney = 40;
        // Подсчет монет при победе
        if (downCount == 0)
        {
            money1 = victoryMoney; // Монеты за победу

            /* Подсчет количества монет за время игры
             * Формула макс_монет_за_время - (время_игры - 10)
             * Монеты начинают терятся таким образом начиная с 10 секунды, каждую секунду игрок теряет монету
             */
            if (time <= 10)
                money2 = timeMoney;
            else
                money2 = timeMoney - (time - 10) > 0 ? timeMoney - (time - 10) : 0;
        }
        else
            money1 = money2 = 0;

        /* Подсчет количества монет за забитые шайбы
         * Формула <максимум монет за голы> / <количество голов суммарно за игру> * <количетво голов забитых игроком>
         * Таким образом если человек будет пропускать шайбы, суммарное количество забитых шайб будет расти дополнительно со стороны ИИ и количество монет будет уменьшаться
         * Начисляет даже в случае поражения
         */
        int playerGoals = downCount == 0 ? goals / 2 + 2 : goals / 2 - 2;
        money3 = goalsMoney / goals * playerGoals;
    }

    IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            ++time;
        }
    }
}
