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
    enum Status : byte
    {
        free,
        keep,
        ready,
        wait
    }

    public List<Checker> checkers; // список всех шайб которыми может играть AI
    public BezierLine line; // нить AI
    private Status statusType = Status.free;

    public bool active;    // false - AI отключен, true - AI включен
    public float speedAI, accuracyAI, timeRest;   // speedAi - скорость AI, accuracyAi - точность AI (разброс в процентах), time - время взятия фишки
    private float leftBorder, rightBorder, upBorder;    // границы бота
    private Vector2 target;     // позиция шайбы для запуска
    private Transform keepObj;  // удерживаемая шайба
    ScreenOptimization screenOpt; 


    private void Start()
    {
        XMLManager.ins.LoadItems();
        speedAI = XMLManager.ins.difficulty.speedAI;
        accuracyAI = XMLManager.ins.difficulty.accuracyAI;
        timeRest = XMLManager.ins.difficulty.timeRest;

        accuracyAI /= 2;
        screenOpt = ScreenOptimization.getInstance();
        upBorder = screenOpt.GetWorldCoord2D(gameObject).first.y;
        leftBorder = Camera.main.ScreenToWorldPoint(new Vector2((0.5f - accuracyAI) * Screen.width, 0)).x;
        rightBorder = Camera.main.ScreenToWorldPoint(new Vector2((0.5f + accuracyAI) * Screen.width, 0)).x;
    }

    void Update()
    {
        active = false;
        if (statusType == Status.keep)
        {
            keepObj.position = Vector2.MoveTowards(keepObj.position, target, Time.deltaTime * speedAI);
            if ((Vector2)keepObj.position == target)
                statusType = Status.ready;
        }

        if (active && statusType == Status.free)
        {
            System.Random random = new System.Random();

            keepObj = checkers[0].objTransform;
            for (int i = 1; i < checkers.Count; ++i)
                if (checkers[i].objTransform.position.y > keepObj.position.y)
                    keepObj = checkers[i].objTransform;

            keepObj.GetComponent<Checker>().OnMouseDown();
            target = new Vector2(UnityEngine.Random.Range(leftBorder, rightBorder), upBorder - 1.2f * keepObj.GetComponent<Checker>().getRadius());

            statusType = Status.keep;
        }

        if (statusType == Status.ready)
        {
            statusType = Status.wait;
            StartCoroutine(delayToPush(0.3f, keepObj, timeRest));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        checkers.Add(col.gameObject.GetComponent<Checker>());
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        checkers.Remove(checkers.Find(item => item.id == col.gameObject.GetComponent<Checker>().id));
    }

    IEnumerator delayToPush(float sec, Transform obj,float timeRest)
    {
        yield return new WaitForSeconds(sec);
        obj.GetComponent<Checker>().OnMouseUp();
        StartCoroutine(delaySec(timeRest));
    }

    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
        statusType = Status.free;   
    }
}
