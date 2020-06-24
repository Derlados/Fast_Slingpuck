using UnityEngine;
using BaseStructures;

public class ScreenOptimization
{
    private static ScreenOptimization instance = null;

    private ScreenOptimization() {}

    public static ScreenOptimization getInstance()
    {
       
        if (instance == null)
            instance = new ScreenOptimization();
        return instance;
    }

    // Установка Collider для объекта в зависимости от разрешения экрана
    public void setColider(GameObject obj, BoxCollider2D box)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 vectSize = rect.anchorMax - rect.anchorMin;
        box.size = new Vector2(vectSize.x * Screen.width, vectSize.y * Screen.height);
    }

    public void setColider(GameObject obj, CircleCollider2D circle)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        circle.radius = Screen.width * (rect.anchorMax.x - rect.anchorMin.x) / 2;
    }

    //Возврат положения объекта в мировых координатах (Левый верхний и правыйнижний угол)
    //Для 2D объектов использующих RectTransform
    public Pair<Vector2, Vector2> GetWorldCoord2D(GameObject obj)
    {
        RectTransform rect = obj.GetComponent<RectTransform>();
        Vector2 first = new Vector2(Screen.width * rect.anchorMin.x, Screen.height * rect.anchorMax.y);
        Vector2 second = new Vector2(Screen.width * rect.anchorMax.x, Screen.height * rect.anchorMin.y);
        return new Pair<Vector2, Vector2>(Camera.main.ScreenToWorldPoint(first), Camera.main.ScreenToWorldPoint(second));
    }
}
