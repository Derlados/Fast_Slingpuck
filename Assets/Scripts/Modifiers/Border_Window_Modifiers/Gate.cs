using UnityEngine;

public class Gate : MonoBehaviour
{
    protected float speedTime = 2f; // Время за которое окно делает полное перемещение с левой границы до правой, разумеется чем меньше это значение тем выше скорость
    protected GameObject window, leftBorder, rightBorder; // окно и границы

    // Функция которая возвращает координату X по которой необходимо будет целится
    // В случае с простым окном вычисления не нужны и можно сразу возвращать координату X

    void Awake()
    {
        // Получаем окно и две границы
        window = gameObject.transform.GetChild(0).gameObject;
        leftBorder = gameObject.transform.GetChild(1).gameObject;
        rightBorder = gameObject.transform.GetChild(2).gameObject;

    }

    public virtual float calculatePos(float sec)
    {
        return window.transform.position.x; 
    }
}
