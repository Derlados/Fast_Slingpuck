using System;
using UnityEngine;
using UnityEngine.UI;

public class Rotation : Modifier
{
    Checker checker;
    GameObject arrowAim;

    private void Start()
    {
        checker = gameObject.GetComponent<Checker>();

        arrowAim = new GameObject("ArrowAim");
        arrowAim.transform.SetParent(this.gameObject.transform);
       
        // Загрузка изображения
        arrowAim.AddComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/levels/checkers/Effect/Arrow");
       
        // Корректировка изображения
        RectTransform rect = arrowAim.GetComponent<RectTransform>();
        float radius = gameObject.GetComponent<CircleCollider2D>().radius;

        rect.localScale = new Vector3(1, 1, 1);  
        arrowAim.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 90));
        rect.sizeDelta = new Vector2(radius * 3, radius * 2);
        rect.localPosition = new Vector3(0, rect.sizeDelta.y / 2, 0);

        Canvas canvas = arrowAim.AddComponent<Canvas>();
        canvas.overrideSorting = true;
        canvas.sortingOrder = -1;

    }

    void Update()
    {
        if (checker.getMouseDown())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.y < checker.playerDownBorder.Down)
            {
                arrowAim.SetActive(true);
                checker.setStop(true);
                checker.angle = calculateRotation(mousePos, checker.transform.position);
                checker.transform.rotation = Quaternion.Euler(0, 0, checker.angle);
            }
            else
            {
                arrowAim.SetActive(false);
                checker.setStop(false);
            }
        }
        else
            arrowAim.SetActive(false);
    }

    /* Функция для вычисления угла поворота шайбы
     * Входные параметры:
     * A - координаты позиции пальца
     * B - координаты позиции шайбы
     * Выходные параметры:
     * Угол поворота, рассчет ведется относительно нижней части поля, для верхней необходимо поменять знак 
     */
    float calculateRotation(Vector2 A, Vector2 B)
    {
        float X, Y; // по сути две стороны прямоугольного треугольника

        X = Math.Abs(A.x - B.x);
        Y = Math.Abs(A.y - B.y);

        float alpha = (float)Math.Atan(X / Y) * 57.3f;

        if (alpha > 45f)
            alpha = 45f;

        if (A.x < B.x)
            return -alpha;
        else
            return alpha;
    }
}
