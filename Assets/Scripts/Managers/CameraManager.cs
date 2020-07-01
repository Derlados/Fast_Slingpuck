using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Canvas Menu, Planets; // Полотоно Меню и Планет соответсвенно
    public Camera thisCamera; // Камера на которую вешается скрипт
    private Vector2 startPos, targetPos; // StartPos - начальная позиция камеры, targetPos - позиция планеты к которой необходимо приблизить камеру
    public float stepMove, stepSize; // stepMove - шаг передвижения камеры , stepSize - шаг приближения камеры
    private GameObject planetLevels;

    Status cameraStatus = Status.freeOnMenu;

    enum Status
    {
        freeOnMenu, // Камера свободна и находится в меню
        freeOnPlanet, // Камера свободна и находится на планете
        zoom, // Камера приближается
    }

    private void Start()
    {
        startPos = thisCamera.transform.position;
        // Оптимизация второго поля под разные екраны так как поле Планет не закреплено за камерой
        Planets.GetComponent<RectTransform>().sizeDelta = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)) * 2;
    }

    private void FixedUpdate()
    {
        if (cameraStatus == Status.zoom)
        {  
            if ((Vector2)thisCamera.transform.position == targetPos)
            {
                cameraStatus = (Vector2)thisCamera.transform.position == startPos ? Status.freeOnMenu : Status.freeOnPlanet;
                if (cameraStatus == Status.freeOnPlanet)
                    planetLevels.SetActive(true);
            }
            else
            {
                Vector2 temp = Vector2.MoveTowards(thisCamera.transform.position, targetPos, stepMove);
                thisCamera.transform.position = new Vector3(temp.x, temp.y, thisCamera.transform.position.z);
                thisCamera.orthographicSize -= stepSize;
            }
        }

    }

    // Приближение к планете
    public void zoomPlanet(GameObject planet)
    {
        if (cameraStatus != Status.freeOnPlanet)
        {
            targetPos = planet.transform.position;
            stepMove = ((Vector2)thisCamera.transform.position - targetPos).magnitude * Time.fixedDeltaTime;
            stepSize = Math.Abs(thisCamera.orthographicSize - 1.5f) * Time.fixedDeltaTime;
            cameraStatus = Status.zoom;
        }
    }

    public void setPlanetsLevels(GameObject planetLevels)
    {
        this.planetLevels = planetLevels;
    }

    // Возврат к стартовому меню

    public void backToStart()
    {
        targetPos = startPos;
        stepMove = ((Vector2)thisCamera.transform.position - targetPos).magnitude * Time.fixedDeltaTime;
        stepSize = -Math.Abs(thisCamera.orthographicSize - 5.05f) * Time.fixedDeltaTime;
        planetLevels.SetActive(false);
        cameraStatus = Status.zoom;    
    }
}

