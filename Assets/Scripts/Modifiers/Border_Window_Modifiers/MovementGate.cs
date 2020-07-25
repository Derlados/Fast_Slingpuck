using System.Collections;
using UnityEngine;

// Движущееся окно
public class MovementGate : Gate
{
    RectTransform rectLeft, rectRight;
    
    float stepScreen, stepWorld; // шаг, для движения за один такт (в экранном режиме и в режиме реального пространтсва)

    Transform leftPos, rightPos, windowPos;
    BoxCollider2D leftBorderCol, rightBorderCol;
    Game game;


    void Start()
    {
        // Объект Game
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();

        // RectTransform окон и границ
        rectLeft = leftBorder.GetComponent<RectTransform>();
        rectRight = rightBorder.GetComponent<RectTransform>();

        // Transform окон и границ
        leftPos = rectLeft.transform;
        rightPos = rectRight.transform;
        windowPos = window.transform;

        // Коллайдеры границ
        leftBorderCol = leftBorder.GetComponent<BoxCollider2D>();
        rightBorderCol = rightBorder.GetComponent<BoxCollider2D>();

        // Шаг для изменения размеров и позиции объектов и границы где может находится окно (последнее необходимо для прогнозирования)
        float left, right;
        left = rectRight.anchorMax.x * Screen.width;
        right = rectLeft.anchorMin.x * Screen.width;


        stepScreen = (left - right) / (speedTime / Time.fixedDeltaTime);
        stepWorld = Camera.main.ScreenToWorldPoint(new Vector2(stepScreen, 0)).x - Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (game.activeGame)
        {
            // Модифицируем объекты по размеру и смещаем позиции
            rectLeft.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectLeft.rect.width - stepScreen);
            leftPos.position = new Vector3(leftPos.position.x - stepWorld / 2, leftPos.position.y, leftPos.position.z);

            rectRight.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectRight.rect.width + stepScreen);
            rightPos.position = new Vector3(rightPos.position.x - stepWorld / 2, rightPos.position.y, rightPos.position.z);

            windowPos.position = new Vector3(windowPos.position.x - stepWorld, windowPos.position.y, windowPos.position.z);

            // Модифицикация колайдеров
            leftBorderCol.size = new Vector2(rectLeft.rect.width, rectLeft.rect.height);
            rightBorderCol.size = new Vector2(rectRight.rect.width, rectRight.rect.height);

            // Смена направления
            if (rectLeft.rect.width < 0 || rectRight.rect.width < 0)
            {
                stepWorld = -stepWorld;
                stepScreen = -stepScreen;
            }
        }
    }

    // Вариант через симуляцию
    public override float calculatePos(float sec)
    {
        float leftSize = rectLeft.rect.width, rightSize = rectRight.rect.width, posX = windowPos.position.x;
        float tempStepWorld = stepWorld, tempStepScreen = stepScreen;

        sec += 0.03f;

        for (float i = 0; i <= sec; i += Time.fixedDeltaTime)
        {
            leftSize -= tempStepScreen;
            rightSize += tempStepScreen;

            posX -= tempStepWorld;

            if (leftSize < 0 || rightSize < 0)
            {
                tempStepWorld = -tempStepWorld;
                tempStepScreen = -tempStepScreen;
            }
        }

        return posX;
    }
}
