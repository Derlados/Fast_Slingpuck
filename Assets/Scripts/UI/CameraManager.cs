using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Canvas Menu, Planets; // Полотоно Меню и Планет соответсвенно
    public Camera thisCamera; // Камера на которую вешается скрипт
    private Vector2 startPos, target; // StartPos - начальная позиция камеры, target - позиция планеты к которой необходимо приблизить камеру
    public float dirrection, speedSize, speedMove; // Скорость приближения камеры. speedSize - скорость изменения размера видимой области, speedMove - скорость перемещения камеры
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

    private void Update()
    {
        if (cameraStatus == Status.zoom)
        {
            Vector2 temp = Vector2.MoveTowards(thisCamera.transform.position, target, Time.deltaTime * speedMove);

            if ((Vector2)thisCamera.transform.position == target)
            {
                cameraStatus = (Vector2)thisCamera.transform.position == startPos ? Status.freeOnMenu : Status.freeOnPlanet;
                if (cameraStatus == Status.freeOnPlanet)
                    planetLevels.SetActive(true);
            }

            thisCamera.transform.position = new Vector3(temp.x, temp.y, thisCamera.transform.position.z);
            thisCamera.orthographicSize -= Time.deltaTime * speedSize * dirrection;
        }

    }

    // Приближение к планете
    public void zoomPlanet(GameObject planet)
    {
        if (cameraStatus != Status.freeOnPlanet)
        {         
            target = planet.transform.position;
            dirrection = 1;
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
        dirrection = -1;
        target = startPos;
        planetLevels.SetActive(false);
        cameraStatus = Status.zoom;    
    }
}

