using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

public class AI : MonoBehaviour
{
    /* Состояния AI
     * free - AI свободен и готов взять следующую шайбу
     * keep - AI удерживает шайбу и ведет её к нити
     * ready - AI готов к запуску шайбы
     * wait - ожидание AI перед запуском следующей шайбы
     */
    protected enum Status : byte
    {
        free,
        keep,
        ready,
        wait,
        aim
    }

    public List<Checker> checkers = new List<Checker>(); // список всех шайб которыми может играть AI
    protected Status statusType = Status.free;

    public bool active;    // false - AI отключен, true - AI включен
    public static float speedAI, accuracyAI, timeRest;  // speedAi - скорость AI, accuracyAi - точность AI (разброс в процентах), time - время взятия фишки, timeAim - время прицеливания
    protected float leftBorder, rightBorder, upBorder;    // границы бота
    protected float angle; // угол поворота шайбы 
    protected Vector2 target;     // позиция шайбы для запуска
    protected Transform keepObj;  // удерживаемая шайба
    protected Checker keepChecker;


    private void Start()
    {
        active = false;

        Difficulty diff = new Difficulty();
      /*if (!XMLManager.LoadData<Difficulty>(ref diff, "settings"))
        {
            XMLManager.LoadDifficulty(ref diff, "begginer");
            XMLManager.SaveData<Difficulty>(diff, "settings");
        }*/

        XMLManager.LoadDifficulty(ref diff, GameRule.difficulties.ToString());

        speedAI = diff.speedAI;
        accuracyAI = diff.accuracyAI;
        timeRest = diff.timeRest;

        accuracyAI /= 2;
        upBorder = ScreenOptimization.GetWorldCoord2D(gameObject).first.y;
        leftBorder = Camera.main.ScreenToWorldPoint(new Vector2((0.5f - accuracyAI) * Screen.width, 0)).x;
        rightBorder = Camera.main.ScreenToWorldPoint(new Vector2((0.5f + accuracyAI) * Screen.width, 0)).x;
    }

    void Update()
    {
        if (active)
        {

            if (statusType == Status.free && checkers.Count > 0)
            {
                getChecker();
                statusType = Status.keep;
            }

            if (statusType == Status.keep)
            {
                keepObj.position = Vector2.MoveTowards(keepObj.position, target, Time.deltaTime * speedAI);
                if ((Vector2)keepObj.position == target)
                    statusType = Status.aim;
            }
            
            if (statusType == Status.aim)
            {
                aim();
                statusType = Status.ready;
            }

            if (statusType == Status.ready)
            {
                statusType = Status.wait;
                StartCoroutine(delayToPush(0.3f, keepObj, timeRest));
            }
        }
    }

    // Выбирает необходимую шайбу и вычисляет позицию на которую она будет поставлена
    public virtual void getChecker()
    {
        keepObj = checkers[0].objTransform;
        for (int i = 1; i < checkers.Count; ++i)
            if (checkers[i].objTransform.position.y > keepObj.position.y)
                keepObj = checkers[i].objTransform;
        keepChecker = keepObj.GetComponent<Checker>();

        keepChecker.OnMouseDown();
        target = new Vector2(UnityEngine.Random.Range(leftBorder, rightBorder), upBorder - 1.2f * keepObj.GetComponent<Checker>().getRadius());
    }

    // Прицеливание, получает точку на которую должна быть направлена шайба, если шайба летит прямо - необходимости в прицеливании нету
    public virtual void aim()
    {

    }

    // Добавляет новую шайбу в список шайб которые может использовать AI
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Modifier>().playableForAI)
            checkers.Add(col.gameObject.GetComponent<Checker>());
    }

    // Удаляет шайбу из списка шайб которые может использовать AI
    private void OnTriggerExit2D(Collider2D col)
    {
        checkers.Remove(checkers.Find(item => item.id == col.gameObject.GetComponent<Checker>().id));
    }

    /* Задержка до запуска шайбы (Иммитация прицеливания) 
     * Параметры:
     * sec - время прицеливания в секундах
     * obj - удерживаемая шайба (компонент Transform)
     * timeRest - время отдыха между запусками шайб
     */
    IEnumerator delayToPush(float sec, Transform obj, float timeRest)
    {
        yield return new WaitForSeconds(sec);
        obj.GetComponent<Checker>().OnMouseUp();
        StartCoroutine(delaySec(timeRest));
    }

    // Задержка в несколько секунд(sec)
    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
        statusType = Status.free;   
    }
}
