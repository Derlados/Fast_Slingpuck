using System;
using System.Collections;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public Canvas GalaxyCanvas; // Полотоно Планет
    public Camera thisCamera; // Камера на которую вешается скрипт
    private Vector2 startPos, targetPos; // StartPos - начальная позиция камеры, targetPos - позиция планеты к которой необходимо приблизить камеру
    private float stepMove, stepSize; // stepMove - шаг передвижения камеры , stepSize - шаг приближения камеры
    public GameObject mainMenu, galaxy; // mainMenu - UI главного меню, galaxy - UI режима прохождения уровней 
    public static GameObject planets;
    public GameObject levelInformation; // Описание уровня
    public GameObject backBtn; //кнопка назад

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

    // Количество планет всего
    public static int allPlanets;

    private void Start()
    {
        cameraStatus = Status.freeOnMenu;
        // Оптимизация второго поля под разные екраны так как поле Планет не закреплено за камерой
        float posX = -Math.Abs(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - (GalaxyCanvas.GetComponent<RectTransform>().sizeDelta.x / 2));
        thisCamera.transform.position = new Vector3(posX, thisCamera.transform.position.y, thisCamera.transform.position.z);

        startPos = thisCamera.transform.position;

        //-2 т.к обьект galaxy содержит еще 2 обьекта, которые не являются планетами
        allPlanets = galaxy.transform.childCount - 2;
        planets = mainMenu;
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
                    backBtn.SetActive(true);
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

    // Загрузка описания уровня
    [System.Serializable]
    public class LevelDesc
    {
        public Image fieldImage;
        public Text GameDifficultyText, GameModeText, GameDescriptionText;
        public Text TargetText1, TargetText2, TargetText3;
    }
    public LevelDesc levelDesc;

    // Окно информации об уровне
    public void loadLevelDesc(Level level)
    {
        levelDesc.fieldImage.sprite = Resources.Load<Sprite>("Sprites/MainMenu/planets/" + level.type.ToString() + "/" + level.type.ToString() + "_planet");

        XElement data; // Данные XML файла
        Debug.Log("XML/Localization/" + LocalizationManager.curLanguage.ToString() + "/Level");
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Lozalization/" + LocalizationManager.curLanguage.ToString() + "/level");
        data = XDocument.Parse(textAsset.text).Element("Level");

        levelDesc.GameDifficultyText.text = data.Element("difficulty").Element(level.difficulties.ToString()).Value;
        levelDesc.GameModeText.text = data.Element("mode").Element(level.mode.ToString()).Value;
        levelDesc.GameDescriptionText.text = data.Element("description").Element(level.mode.ToString()).Value;

        string el = GameRule.ActiveAI ? level.mode.ToString() + "AI" : level.mode.ToString();
        data = data.Element("targets").Element(el);

        levelDesc.TargetText1.text = data.Element("target1").Value.Replace("NUMBER", GameRule.target1.ToString());
        levelDesc.TargetText2.text = data.Element("target2").Value.Replace("NUMBER", GameRule.target2.ToString());
        levelDesc.TargetText3.text = data.Element("target3").Value.Replace("NUMBER", GameRule.target3.ToString());

        levelInformation.SetActive(true);
        backBtn.SetActive(false);
    }

    // Загрузка игры
    public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }

    // Приближение к планете
    public void zoomPlanet(GameObject planet)
    {
        if (cameraStatus != Status.freeOnPlanet)
        {
            targetPos = planet.transform.position;
            stepMove = ((Vector2)thisCamera.transform.position - targetPos).magnitude * Time.fixedDeltaTime;
            stepSize = Math.Abs(thisCamera.orthographicSize - 1.18f) * Time.fixedDeltaTime;
            cameraStatus = Status.zoom;

            //сохраняем данные о планете перед изменением размеров планеты
            tmp = planet.GetComponent<RectTransform>().localScale;
            planetTmp = planet;
            StartCoroutine(scalePlanet(planet,true));

            for (int i = 2; i < galaxy.transform.childCount; ++i)
                if (galaxy.transform.GetChild(i).transform.gameObject != planet.transform.gameObject)
                    StartCoroutine(fadePlanet(i, true));
        }
    }

    public void setPlanetsLevels(GameObject planetLevels)
    {
        this.planetLevels = planetLevels;
    }

    public void setPlanetNumber(int num)
    {
        numberPlanet = num;
        GameRule.planetNum = num;
    }

    // Возврат к стартовому меню
    public void backToStart()
    {
        targetPos = startPos;
        stepMove = ((Vector2)thisCamera.transform.position - targetPos).magnitude * Time.fixedDeltaTime;
        stepSize = -Math.Abs(thisCamera.orthographicSize - 5.05f) * Time.fixedDeltaTime;
        planetLevels.SetActive(false);
        backBtn.SetActive(false);
        cameraStatus = Status.zoom;
        StartCoroutine(scalePlanet(planetTmp, false));

        for (int i = 2; i < galaxy.transform.childCount; ++i)
            if (galaxy.transform.GetChild(i) != planetTmp)
                StartCoroutine(fadePlanet(i, false));
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
    /*
     * planet - обьект планеты
     * to = true - уменьшение к размеру 1.15
     * to = false - возращение к прежнему размеру
     */
    IEnumerator scalePlanet(GameObject planet,bool to)
    {
        Vector3 temp = planet.GetComponent<RectTransform>().localScale;
        float toScale;
   
        if (to) toScale = 1.15f;
        else toScale = tmp.x;

        if (temp.x < toScale)
        {
            while (temp.x <= toScale)
            {
                temp.x += Time.deltaTime / 2.5f;

                temp = new Vector3(temp.x, temp.x, temp.x);
                planet.GetComponent<RectTransform>().localScale = temp;
                yield return null;
            }
        }
        else
        {
            while (temp.x > toScale)
            {
                temp.x -= Time.deltaTime / 1;

                temp = new Vector3(temp.x, temp.x, temp.x);
                planet.GetComponent<RectTransform>().localScale = temp;
                yield return null;
            }
        }
    }

    // Анимация затухания планет
    /*
     * num - номер планеты в галактике планеты
     * to = true - затухание планеты 
     * to = false - разтухание планеты
     */
    IEnumerator fadePlanet(int num, bool to)
    {
        Color color = galaxy.transform.GetChild(num).transform.GetComponent<Image>().color;

        if (to)
        {
            while (galaxy.transform.GetChild(num).transform.GetComponent<Image>().color.a > 0)
            {               
                color.a -= Time.deltaTime / 1; 
                galaxy.transform.GetChild(num).transform.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);
                yield return null;
            }
        }
        else
        {
            while (galaxy.transform.GetChild(num).transform.GetComponent<Image>().color.a < 1)
            {                  
                color.a += Time.deltaTime / 1;  
                galaxy.transform.GetChild(num).transform.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);
                yield return null;
            }
        }
     

    }

}

