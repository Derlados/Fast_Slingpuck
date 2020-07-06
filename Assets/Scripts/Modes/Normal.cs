using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Normal : MonoBehaviour, Mode
{
    GameObject checkers;
    GameObject AI;
    GameObject capperField;
    GameObject gameMenu;
    Game game;

    // Счетчики
    public int score;
    public byte upCount = 4, downCount = 4; // константы (4) необходимо заменить

    // Текст счетчиков
    public Text upCountText, downCountText, gameCounter;

    // Монеты
    int money;

    void Start()
    {
        initScene();
        StartCoroutine(delayBeforeStart(3));
        StartCoroutine(delayAI(3));
    }

    public void initScene()
    {
        game = GetComponent<Game>();

        // Бот
        AI = game.AI;

        // Текст счетчиков
        upCountText = game.upCountText;
        downCountText = game.downCountText;
        gameCounter = game.gameCounter;

        // Меню
        gameMenu = game.gameMenu;

        // Заглушка
        capperField = game.capperField;
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

    // Окончание игры
    public void gameOver()
    {
        AI.GetComponent<AI>().active = false;
        gameMenu.GetComponent<GameMenu>().gameOver(downCount == 0 ? "YOU WIN !" :  "YOU LOSE !", 0);
    }

    public void calculateResult()
    {

    }
}
