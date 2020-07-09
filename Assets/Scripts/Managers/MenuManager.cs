using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Canvas GalaxyCanvas; // Полотоно Планет
    public Camera thisCamera,tmpCamera; // Камера на которую вешается скрипт
    private Vector2 startPos, targetPos; // StartPos - начальная позиция камеры, targetPos - позиция планеты к которой необходимо приблизить камеру
    private float stepMove, stepSize; // stepMove - шаг передвижения камеры , stepSize - шаг приближения камеры
    public GameObject mainMenu, galaxy; // mainMenu - UI главного меню, galaxy - UI режима прохождения уровней 

    // Уровни планеты и номер самой планеты
    private GameObject planetLevels;
    private int numberPlanet;

    // Статус действий камеры
    public enum Status
    {
        freeOnMenu, // Камера свободна и находится в меню
        freeOnPlanet, // Камера свободна и находится на планете
        zoom, // Камера приближается
    }
    public static Status cameraStatus = Status.freeOnMenu;

    //сохранение планеты перед увелечением\отдалением
    Vector3 tmp; //сохраненный размер
    GameObject planetTmp; //сохраненная планета

    private void Start()
    {
        // Оптимизация второго поля под разные екраны так как поле Планет не закреплено за камерой
        float posX = -Math.Abs(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - (GalaxyCanvas.GetComponent<RectTransform>().sizeDelta.x / 2));
        thisCamera.transform.position = new Vector3(posX, thisCamera.transform.position.y, thisCamera.transform.position.z);

        startPos = thisCamera.transform.position;
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            backToStart();
        }

        if (cameraStatus == Status.zoom)
        {  
            if ((Vector2)thisCamera.transform.position == targetPos)
            {
                cameraStatus = (Vector2)thisCamera.transform.position == startPos ? Status.freeOnMenu : Status.freeOnPlanet;
                if (cameraStatus == Status.freeOnPlanet)
                {
                    planetLevels.SetActive(true);
                    StartCoroutine(Spawn(planetLevels, numberPlanet));
                }
            }
            else
            {
                Vector2 temp = Vector2.MoveTowards(thisCamera.transform.position, targetPos, stepMove);
                thisCamera.transform.position = new Vector3(temp.x, temp.y, thisCamera.transform.position.z);
                thisCamera.orthographicSize -= stepSize;
            }
        }

    }

    //////////////////////////////////////////////////////////////////////////////////  MAIN MENU ///////////////////////////////////////////////////////////////////////////////////
   
    public void chooseGalaxy()
    {
        mainMenu.SetActive(false);
        galaxy.SetActive(true);
    }

    //////////////////////////////////////////////////////////////////////////////////  SHOP MENU ///////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////  GALAXY MENU ///////////////////////////////////////////////////////////////////////////////////

    // Приближение к планете
    public void zoomPlanet(GameObject planet)
    {
        if (cameraStatus != Status.freeOnPlanet)
        {
            targetPos = planet.transform.position;
            tmpCamera = thisCamera;
            thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, thisCamera.transform.position.y, planet.transform.position.z-2f);

            stepMove = ((Vector2)thisCamera.transform.position - targetPos).magnitude * Time.fixedDeltaTime;
            stepSize = Math.Abs(thisCamera.orthographicSize - 1.18f) * Time.fixedDeltaTime;
            cameraStatus = Status.zoom;

            //сохраняем данные о планете перед изменением размеров планеты
            tmp = planet.GetComponent<RectTransform>().localScale;
            planetTmp = planet;

            StartCoroutine(scalePlanet(planet,true));
        }
    }

    public void setPlanetsLevels(GameObject planetLevels)
    {
        this.planetLevels = planetLevels;
    }

    public void setPlanetNumber(int num)
    {
        numberPlanet = num;
    }

    // Возврат к стартовому меню
    public void backToStart()
    {
        targetPos = startPos;
        thisCamera.transform.position = new Vector3(thisCamera.transform.position.x, thisCamera.transform.position.y, tmpCamera.transform.position.z - 2f);

        stepMove = ((Vector2)thisCamera.transform.position - targetPos).magnitude * Time.fixedDeltaTime;
        stepSize = -Math.Abs(thisCamera.orthographicSize - 5.05f) * Time.fixedDeltaTime;
        planetLevels.SetActive(false);
        cameraStatus = Status.zoom;
        StartCoroutine(scalePlanet(planetTmp, false));
    }

    // Анимация прорисовки уровней
    IEnumerator Spawn(GameObject gameObject, int num)
    {
        float timeDelay;

        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            GameObject ChildLvl = gameObject.transform.GetChild(i).gameObject;
            byte progress = PlayerData.getInstance().progress[num][i];

            for (int j = 0; j < ChildLvl.transform.childCount; ++j)
            {
                timeDelay = 0.1f;
                GameObject child = ChildLvl.transform.GetChild(j).gameObject;

                if (progress != 0)
                {
                    if (j == 0)
                        for (int k = 1; k <= progress; ++k)
                            child.transform.GetChild(k).gameObject.SetActive(true);
                    child.gameObject.SetActive(true);
                }
                else
                {
                    if (j == 0 && (i == 0 || PlayerData.getInstance().progress[num][i - 1] != 0))
                        child.gameObject.SetActive(true);
                    else if (j == 0)
                    {
                        Color32 thisColor = child.GetComponent<Image>().color;
                        child.GetComponent<Image>().color = new Color32(thisColor.r, thisColor.g, thisColor.b, 170);
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        child.gameObject.SetActive(false);
                        timeDelay = 0;
                    }
                }

                yield return new WaitForSeconds(timeDelay);
            }
        }
    }

    // Анимация увеличения\уменьшения планет
    IEnumerator scalePlanet(GameObject planet,bool to)
    {
        Vector3 temp = planet.GetComponent<RectTransform>().localScale;
        float toScale;

        if (to) toScale = 1.15f;
        else toScale = tmp.x;

        if (temp.x < toScale)
        {
            for (float i = temp.x; i <= toScale; i = i + 0.01f)
            {
                temp = new Vector3(i, i, i);
                planet.GetComponent<RectTransform>().localScale = temp;
                yield return new WaitForSeconds(0.020f);
            }
        }
        else
        {
            for (float i = temp.x; i >= toScale; i = i - 0.01f)
            {
                temp = new Vector3(i, i, i);
                planet.GetComponent<RectTransform>().localScale = temp;
                yield return new WaitForSeconds(0.020f);
            }
        }
    }
}

