using UnityEngine;
using BaseStructures;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public static class ScreenOptimization
{
    // Установка Collider для объекта в зависимости от разрешения экрана
    public static void setColider(GameObject obj, BoxCollider2D box)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 vectSize = rect.anchorMax - rect.anchorMin;
        box.size = new Vector2(vectSize.x * Screen.width, vectSize.y * Screen.height);
    }

    public static void setColider(GameObject obj, CircleCollider2D circle)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        circle.radius = Screen.width * (rect.anchorMax.x - rect.anchorMin.x) / 2;
    }

    // Установка размера для объекта в проценте от разрешения экрана
    public static void setSize(GameObject obj, CircleCollider2D circle, float percent)
    {
        obj.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * percent, Screen.width * percent);
        circle.radius = (Screen.width *  percent) / 2;
    }

    //Возврат положения объекта в мировых координатах (Левый верхний и правыйнижний угол)
    //Для 2D объектов использующих RectTransform
    public static Pair<Vector2, Vector2> GetWorldCoord2D(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 first = new Vector2(Screen.width * rect.anchorMin.x, Screen.height * rect.anchorMax.y);
        Vector2 second = new Vector2(Screen.width * rect.anchorMax.x, Screen.height * rect.anchorMin.y);
        return new Pair<Vector2, Vector2>(Camera.main.ScreenToWorldPoint(first), Camera.main.ScreenToWorldPoint(second));
    }

    //установка радиуса неона относительно разрешения экрана
    public static void setNeonRadius(GameObject obj)
    {
        Volume volume = obj.transform.GetComponent<Volume>();
        volume.sharedProfile.TryGet<Bloom>(out Bloom bloom);

        double delta = (double)Screen.height / (double)Screen.width;
        bloom.threshold.SetValue(new MinFloatParameter((float)Math.Exp(1.0892182f + 0.37382208f * delta / Math.Log(delta)), 0, true));
        bloom.intensity.SetValue(new MinFloatParameter((float)Math.Exp(2.0676649f - 1.5169059f * Math.Log(delta)), 0, true));
        bloom.scatter.SetValue(new MinFloatParameter((float)(0.70737272f - 1.0811167f / delta), 0, true));
    }

    public static void fix18_9(BoxCollider2D box)
    {
        box.size = new Vector2(box.size.x * 0.935f, box.size.y);
    }

    public static void fix18_9(EdgeCollider2D edge)
    {
        for (int i = 0; i < edge.points.Length; ++i)
            edge.points[i] = new Vector2(edge.points[i].x * 0.935f, edge.points[i].y);
    }
}
