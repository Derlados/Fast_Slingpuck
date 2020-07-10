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

    // Start is called before the first frame update
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

        downBorderHolder = game.downBorderHolder;

        // Меню
        gameMenu = game.gameMenu;

        // Заглушка
        capperField = game.capperField;
        game.checkersSpeed.SetActive(true);
    }

    public void changeCount(GameObject obj)
    {
        if (obj.transform.position.y > 0 && !obj.GetComponent<Modifier>().playableForAI)
            downCountText.text = (++downCount).ToString();
        else if (obj.transform.position.y < 0 && obj.GetComponent<Modifier>().playableForAI)
            upCountText.text = (++upCount).ToString();
    }

    public void gameOver()
    {
        AI.GetComponent<AI>().active = false;
        gameMenu.GetComponent<GameMenu>().gameOver("Game Over !", downCount);
    }

    public void calculateResult()
    {

    }

    // Задержка перед началом игры
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
