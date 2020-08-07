using System;
using System.Collections;
using UnityEngine;

// Движущееся окно
public class MovementGate : Gate
{
    protected float stepScreen, stepWorld; // шаг, для движения за один такт (в экранном режиме и в режиме реального пространтсва)

    protected Transform leftPos, rightPos, windowPos;
    protected BoxCollider2D leftBorderCol, rightBorderCol;
    protected Vector2 worldLeftBorderWindow, worldRightBorderWindow, currentTarget;
    protected float screenLeftBorderWindow, screenRightBorderWindow;
    protected RectTransform rectLeftBorder, rectRightBorder, rectWindow;

    public void Start()
    {
        rectWindow = window.GetComponent<RectTransform>();
        rectLeftBorder = leftBorder.GetComponent<RectTransform>();
        rectRightBorder = rightBorder.GetComponent<RectTransform>();

        // Границы в экранном пространстве
        screenLeftBorderWindow = Screen.width * rectLeftBorder.anchorMin.x + rectWindow.rect.width / 2;
        screenRightBorderWindow = Screen.width * rectRightBorder.anchorMax.x - rectWindow.rect.width / 2;

        // Границы в мировых координатах
        worldLeftBorderWindow =  new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(screenLeftBorderWindow, 0)).x, 0);
        worldRightBorderWindow = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(screenRightBorderWindow, 0)).x, 0);

        // Увеличение размера границ в два раза (потому что идет смещение всех ворот сразу, отстальная часть за экраном)
        rectLeftBorder.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectLeftBorder.rect.width * 2f);
        rectRightBorder.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rectRightBorder.rect.width * 2f);

        // Смещение позиции границ (для корректного отображения)
        leftBorder.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * rectLeftBorder.anchorMin.x, 0)).x, leftBorder.transform.position.y);
        rightBorder.transform.position = new Vector2(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width * rectRightBorder.anchorMax.x, 0)).x, rightBorder.transform.position.y);

        // Модифицикация колайдеров
        BoxCollider2D leftBorderCol = leftBorder.GetComponent<BoxCollider2D>(), rightBorderCol = rightBorder.GetComponent<BoxCollider2D>();
        leftBorderCol.size = new Vector2(leftBorderCol.size.x * 2, leftBorderCol.size.y);
        rightBorderCol.size = new Vector2(rightBorderCol.size.x * 2, rightBorderCol.size.y); ;

        // Шаг для изменения размеров и позиции объектов и границы где может находится окно (последнее необходимо для прогнозирования)
        float left, right;
        left = rectRightBorder.anchorMax.x * Screen.width;
        right = rectLeftBorder.anchorMin.x * Screen.width;


        stepScreen = (left - right) / (speedTime / Time.fixedDeltaTime);
        stepWorld = Camera.main.ScreenToWorldPoint(new Vector2(stepScreen, 0)).x - Camera.main.ScreenToWorldPoint(new Vector2(0, 0)).x;

        currentTarget = worldLeftBorderWindow; // Текущее движение окна (в левую сторону)
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Game.activeGame)
        {
            gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, currentTarget, stepWorld);

            // Смена направления
            if ((Vector2)gameObject.transform.position == worldLeftBorderWindow)
                currentTarget = worldRightBorderWindow;
            else if ((Vector2)gameObject.transform.position == worldRightBorderWindow)
                currentTarget = worldLeftBorderWindow;

        }
    }

    // Симуляция для прогнозирования
    public override float calculatePos(float sec)
    {
        Vector2 currTarget = currentTarget;
        float posX = gameObject.transform.position.x, stWorld = stepWorld;

        sec += 0.03f;

        if (currTarget == worldRightBorderWindow)
            stWorld = -stWorld;


        for (float i = 0; i <= sec; i += Time.fixedDeltaTime)
        {
            posX -= stWorld;

            if (posX <= worldLeftBorderWindow.x || posX >= worldRightBorderWindow.x)
                stWorld = -stWorld;
        }

        return posX;
    }
}
