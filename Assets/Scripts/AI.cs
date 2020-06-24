using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

    public List<GameObject> checkers; // список всех шайб которыми может играть AI
    public BezierLine line;
    private Status statusType = Status.free;

    private bool active; // false - AI отключен, true - AI включен
    public float leftBorder, rightBorder, upBorder;
    private Vector2 target;
    private Transform keepObj;
    ScreenOptimization screenOpt;


    private void Start()
    {
        screenOpt = ScreenOptimization.getInstance();
        upBorder = screenOpt.GetWorldCoord2D(gameObject).first.y;
        leftBorder = Camera.main.ScreenToWorldPoint(new Vector2(0.25f * Screen.width, 0)).x;
        rightBorder = Camera.main.ScreenToWorldPoint(new Vector2(0.75f * Screen.width, 0)).x;
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            active = true;
        }

        if (statusType == Status.keep)
        {
            keepObj.position = Vector2.MoveTowards(keepObj.position, target, Time.deltaTime);
            if ((Vector2)keepObj.position == target)
                statusType = Status.ready;
        }

        if (active && statusType == Status.free)
        {
            active = false;
            System.Random random = new System.Random();

            keepObj = checkers[random.Next(0, checkers.Count)].GetComponent<Transform>();
            keepObj.GetComponent<Checker>().Keep();
            target = new Vector2(UnityEngine.Random.Range(leftBorder, rightBorder), upBorder - 1.2f * keepObj.GetComponent<Checker>().getRadius());

            statusType = Status.keep;
        }

        if (statusType == Status.ready)
        {
            statusType = Status.wait;
            StartCoroutine(delayToPush(0.3f, keepObj));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        checkers.Add(col.gameObject);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        checkers.Remove(checkers.Find(item => item.GetComponent<Checker>().id == col.gameObject.GetComponent<Checker>().id));
    }

    IEnumerator delayToPush(float sec, Transform obj)
    {
        yield return new WaitForSeconds(sec);
        obj.GetComponent<Checker>().OnMouseUp();
        StartCoroutine(delaySec(1));
    }

    IEnumerator delaySec(float sec)
    {
        yield return new WaitForSeconds(sec);
        statusType = Status.free;   
    }
}
