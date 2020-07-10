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

    // Счетчики
    public int score;
    public byte upCount = 4, downCount = 4; // константы (4) необходимо заменить

    // Текст счетчиков
    public Text upCountText, downCountText, gameCounterText;

    // Монеты
    int money;
   
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
        if (!GameRule.AI)
            AI.SetActive(false);

        // Текст счетчиков
        upCountText = game.upCountText;
        downCountText = game.downCountText;
        gameCounterText = game.gameCounter;

        // Меню
        gameMenu = game.gameMenu;

        // Заглушка
        capperField = game.capperField;
        game.checkersNormal.SetActive(true);
    }

    // Задержка перед стартом игры
    IEnumerator delayBeforeStart(int sec)
    {
        for (int i = sec; i >= 1; --i)
        {
            gameCounterText.text = i.ToString();
            yield return new WaitForSeconds(1);
        }

        gameCounterText.text = "GO!"; // Заменить и локализовать
        capperField.SetActive(false);
        AI.GetComponent<AI>().active = true;
        yield return new WaitForSeconds(1);
    }


    /* Функция счета очков при удачном попадании в "окно"
    * Параметры:
    * obj - шайба которая пролетела через окно
    */
    public void changeCount(GameObject obj)
    {
        if (obj.transform.position.y > 0)
        {
            --downCount;
            ++upCount;
            money += 100;
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
        gameMenu.GetComponent<GameMenu>().gameOver(downCount == 0 ? "YOU WIN !" :  "YOU LOSE !", money);
        game.ChangeParticle(GameRule.type.ToString() + "_particle", false);
    }

    public void calculateResult()
    {

    }
}
