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
    public static float speedAI, accuracyAI, timeRest, timeAim = 0.3f;  // speedAi - скорость AI, accuracyAi - точность AI (разброс в процентах), time - время взятия фишки, timeAim - время прицеливания
    protected float dispersion, upBorder;    // границы бота
    protected float angle; // угол поворота шайбы 
    protected float leftBorder, rightBorder; // границы разброса точности (разброс куда бот может попасть при запуске шайбы)
    protected Vector2 moveTarget, aimTarget; // позиция шайбы для запуска и точка в которую целится AI 
    protected Transform keepObj;  // удерживаемая шайба

    // Объекты необходимые AI для анализа
    protected Checker keepChecker;
    protected Gate gate;
    protected BezierLine AIString;

    Game game;

    private void Start()
    {
        AIString = GameObject.FindGameObjectWithTag("UpString").GetComponent<BezierLine>();
        game = GameObject.FindGameObjectWithTag("Game").GetComponent<Game>();
        gate = game.gate.GetComponent<Gate>();

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
        dispersion = Camera.main.ScreenToWorldPoint(new Vector2(accuracyAI * Screen.width, 0)).x + Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x;
    }

    void FixedUpdate()
    {
        if (active)
        {

            if (statusType == Status.free && checkers.Count > 0)
            {
                getChecker();
                getTarget();
                statusType = Status.keep;
            }       
            
            if (statusType == Status.aim)
            {
                aim();
                statusType = Status.ready;
            }

            if (statusType == Status.ready)
            {
                statusType = Status.wait;
                StartCoroutine(delayToPush(timeAim, keepObj, timeRest));
            }

            if (statusType == Status.keep)
            {
                if ((Vector2)keepObj.position == moveTarget)
                    statusType = Status.aim;
                keepObj.position = Vector2.MoveTowards(keepChecker.transform.position, moveTarget, Time.fixedDeltaTime * speedAI);
            }
        }
    }

    // Выбирает необходимую шайбу
    public void getChecker()
    {
        keepObj = checkers[0].objTransform;
        for (int i = 1; i < checkers.Count; ++i)
            if (checkers[i].objTransform.position.y > keepObj.position.y)
                keepObj = checkers[i].objTransform;
        keepChecker = keepObj.GetComponent<Checker>();

        keepChecker.OnMouseDown();
        keepChecker.angle = 270f; // Иногда происходит баг, что шайба не успевает переключится и не разворачивается на 270 градусов у AI
    }

    // Функция возвращающаяя цель, куда необходимо целится  и куда поставить шайбу
    public virtual void getTarget()
    {
        Checker.Border border = keepChecker.playerUpBorder;
        //keepTime - время за которое бот тянет  ̶л̶я̶м̶к̶у̶  шайбу на позицию
        float keepTime = (((Vector2)keepObj.position - new Vector2(keepObj.position.x, border.Up)).magnitude / (Time.fixedDeltaTime * speedAI)) * Time.fixedDeltaTime * 1.2f;
        // 0.14f - примерно за столько времени у AI всегда летит шайба, если будет изменяться скорость - необходимо будет исправить (да, это пока что такой костыль)
        float posX = gate.calculatePos(timeAim + keepTime + 0.14f);

        leftBorder = posX - dispersion;
        rightBorder = posX + dispersion;

        float coordY = AIString.coordY + (border.Up - (AIString.coordY - AIString.correction)) * (keepChecker.GetComponent<Rigidbody2D>().drag / Checker.DRAG);

        aimTarget = moveTarget = new Vector2(UnityEngine.Random.Range(border.Left < leftBorder ? leftBorder : border.Left, border.Right > rightBorder ? rightBorder : border.Right), coordY);
        Debug.Log(moveTarget);
    }

    // Прицеливание, получает точку на которую должна быть направлена шайба, если шайба летит прямо - необходимости в прицеливании нету
    public virtual void aim()
    {
      
    }

    // Добавляет новую шайбу в список шайб которые может использовать AI
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.GetComponent<Checker>().playableForAI)
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
        keepChecker.OnMouseUp();
        Debug.Log(keepChecker.transform.position);
        StartCoroutine(delaySec(timeRest));
    }

    // Задержка в несколько секунд(sec)
    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
        statusType = Status.free;   
    }
}
