using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Canvas GalaxyCanvas; // Полотоно Планет
    public Camera thisCamera; // Камера на которую вешается скрипт
    private Vector2 startPos, targetPos; // StartPos - начальная позиция камеры, targetPos - позиция планеты к которой необходимо приблизить камеру
    private float stepMove, stepSize; // stepMove - шаг передвижения камеры , stepSize - шаг приближения камеры
    private GameObject planetLevels; 
    public GameObject mainMenu, galaxy; // mainMenu - UI главного меню, galaxy - UI режима прохождения уровней 

    Status cameraStatus = Status.freeOnMenu;

    enum Status
    {
        freeOnMenu, // Камера свободна и находится в меню
        freeOnPlanet, // Камера свободна и находится на планете
        zoom, // Камера приближается
    }

    private void Start()
    {
        // Оптимизация второго поля под разные екраны так как поле Планет не закреплено за камерой
        float posX = -Math.Abs(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - (GalaxyCanvas.GetComponent<RectTransform>().sizeDelta.x / 2));
        thisCamera.transform.position = new Vector3(posX, thisCamera.transform.position.y, thisCamera.transform.position.z);

        startPos = thisCamera.transform.position;
    }

    private void FixedUpdate()
    {
        if (cameraStatus == Status.zoom)
        {  
            if ((Vector2)thisCamera.transform.position == targetPos)
            {
                cameraStatus = (Vector2)thisCamera.transform.position == startPos ? Status.freeOnMenu : Status.freeOnPlanet;
                if (cameraStatus == Status.freeOnPlanet)
                {
                    planetLevels.SetActive(true);
                    StartCoroutine(Spawn(planetLevels));
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

    IEnumerator Spawn(GameObject gameObject)
    {
        for (int i = 0; i < gameObject.transform.childCount; ++i)
        {
            GameObject ChildLvl = gameObject.transform.GetChild(i).gameObject;
            bool progress = PlayerData.getInstance().progress[Int32.Parse(gameObject.name)][i];

            for (int j = 0; j < ChildLvl.transform.childCount; ++j)
            {
                GameObject child = ChildLvl.transform.GetChild(j).gameObject;

                if (!progress)
                {
                    Color32 thisColor = child.GetComponent<Image>().color;
                    child.GetComponent<Image>().color = new Color32(thisColor.r, thisColor.g, thisColor.b, 170);
                }

                child.gameObject.SetActive(true);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

