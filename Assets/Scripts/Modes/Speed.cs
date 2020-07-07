using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BaseStructures;

public class Speed : MonoBehaviour, Mode
{
    GameObject AI; // Ситуативно может быть, а может и не быть
    GameObject capperField;
    GameObject gameMenu;
    GameObject speedGameChecker;
    GameObject downBorderHolder;
    Game game;

    Text upCountText, downCountText, gameCounterText;

    // Счетчики
    public int score;
    public short upCount = 0, downCount = 0; // константы (4) необходимо заменить

    // Start is called before the first frame update
    void Start()
    {
        initScene();
        delayBeforeStart(3);
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
        game.speedGameChecker.SetActive(true);
    }

    public void changeCount(bool direction)
    {
        if (direction)
            downCountText.text = (++downCount).ToString();
        else
            upCountText.text = (++upCount).ToString();

        RandomPosition();
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
        
        if (AI != null)
            AI.GetComponent<AI>().active = true;
        
        yield return new WaitForSeconds(1);

        StartCoroutine(counter(60));
    }

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

    //установка шайбы в рандомное место в нижнем поле
    void RandomPosition()
    {
        Pair<Vector2, Vector2> points;
        points = ScreenOptimization.GetWorldCoord2D(downBorderHolder);
        Vector2 randomPos = new Vector2(Random.Range(points.first.x, points.second.x), Random.Range(points.first.y, points.second.y));
        speedGameChecker.transform.position = randomPos;
    }
}
