using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAI : AI
{
    public override void aim()
    {
        keepChecker.angle += calculateAngle(aimTarget, new Vector2(moveTarget.x, 0));
        Debug.Log(keepChecker.angle);
        keepObj.rotation = Quaternion.Euler(0, 0, keepChecker.angle);
    }
    public override void getTarget()
    {
        Checker.Border border = keepChecker.playerUpBorder;
        moveTarget = new Vector2(UnityEngine.Random.Range(border.Left, border.Right), border.Up);
        //keepTime - время за которое бот тянет  ̶л̶я̶м̶к̶у̶  шайбу на позицию
        float keepTime = (((Vector2)keepObj.position - moveTarget).magnitude / (Time.fixedDeltaTime * speedAI)) * Time.fixedDeltaTime * 1.2f;
        // 0.14f - примерно за столько времени у AI всегда летит шайба, если будет изменяться скорость - необходимо будет исправить (да, это пока что такой костыль)
        float posX = gate.calculatePos(timeAim + keepTime + 0.14f);

        leftBorder = posX - dispersion;
        rightBorder = posX + dispersion;
        aimTarget = new Vector2(UnityEngine.Random.Range(border.Left < leftBorder ? leftBorder : border.Left, border.Right > rightBorder ? rightBorder : border.Right), border.Up);
    }


    /* Подсчет угла поворота
     * Входные параметры:
     * A - координаты позиции шайбы
     * B - координаты цели
     * Выходыне параметры:
     * Угол поворота в градусах
     */
    private float calculateAngle(Vector2 A, Vector2 B)
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
