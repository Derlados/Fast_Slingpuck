using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject window, leftBorder, rightBorder; // окно и границы
    RectTransform rectLeft, rectRight, rectWindow;
    float speedTime = 2f; // Время за которое окно делает полное перемещение с левой границы до правой, разумеется чем меньше это значение тем выше скорость
    float stepScreen, stepWorld; // шаг, для движения за один такт (в экранном режиме и в режиме реального пространтсва)

    Transform leftPos, rightPos, windowPos;
    BoxCollider2D leftBorderCol, rightBorderCol;
    Game game;

    bool debug = false;

    float leftWindow, rightWindow; // Границы координат по X, на которые может сдвинуться окно

    void Start()
    {
        // Получаем окно и две границы
        window = gameObject.transform.GetChild(0).gameObject;
        leftBorder = gameObject.transform.GetChild(1).gameObject;
        rightBorder = gameObject.transform.GetChild(2).gameObject;

        // Объект Game
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();

        // RectTransform окон и границ
        rectLeft = leftBorder.GetComponent<RectTransform>();
        rectRight = rightBorder.GetComponent<RectTransform>();
        rectWindow = window.GetComponent<RectTransform>();

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
    public void calculatePos(float sec)
    {
        float leftSize = rectLeft.rect.width, rightSize = rectRight.rect.width, posX = windowPos.position.x;
        float tempStepWorld = stepWorld, tempStepScreen = stepScreen;

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
    }

}
