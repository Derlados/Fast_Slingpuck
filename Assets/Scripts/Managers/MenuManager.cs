using BaseStructures;
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
    public static GameObject planets; //обьект mainMenu
    public GameObject levelInformation; // Описание уровня
    public GameObject backBtn; //кнопка назад
    public static GameObject menu; //обьект меню
    public static Level level; //текущий уровень

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

    private void Awake()
    {
        allPlanets = galaxy.transform.childCount - 2;
        planets = mainMenu;
    }

    private void Start()
    {
        cameraStatus = Status.freeOnMenu;
        // Оптимизация второго поля под разные екраны так как поле Планет не закреплено за камерой
        //float posX = -Math.Abs(Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x - (GalaxyCanvas.GetComponent<RectTransform>().sizeDelta.x / 2)); // Сдвиг карты к левому краю второго поля 
        thisCamera.transform.position = new Vector3(0, thisCamera.transform.position.y, thisCamera.transform.position.z);

        startPos = thisCamera.transform.position;

        //-2 т.к обьект galaxy содержит еще 2 обьекта, которые не являются планетами
        setPlanetProgress();

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

    //////////////////////////////////////////////////////////////////////////////////  Main menu  ///////////////////////////////////////////////////////////////////////////////////

    public void setMenu(GameObject menu)
    {
        MenuManager.menu = menu;
    }
    public void btn(bool active)
    {
        menu.SetActive(active);
        thisCamera.transform.GetComponent<UIParallax>().setActive(!active);
        AudioManager.PlaySound(AudioManager.Audio.click);
    }

    //////////////////////////////////////////////////////////////////////////////////  Galaxy  ///////////////////////////////////////////////////////////////////////////////////

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
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Lozalization/" + PlayerData.getInstance().lang.ToString() + "/level");
        data = XDocument.Parse(textAsset.text).Element("Level");

        levelDesc.GameDifficultyText.text = data.Element("difficulty").Element(level.difficulties.ToString()).Value;
        levelDesc.GameModeText.text = data.Element("mode").Element(level.mode.ToString()).Value;
        levelDesc.GameDescriptionText.text = data.Element("description").Element(level.mode.ToString()).Value;

        string el = GameRule.TypeAI != GameRule.AI.None ? level.mode.ToString() + "AI" : level.mode.ToString();
        data = data.Element("targets").Element(el);

        levelDesc.TargetText1.text = data.Element("target1").Value.Replace("NUMBER", GameRule.target1.ToString());
        levelDesc.TargetText2.text = data.Element("target2").Value.Replace("NUMBER", GameRule.target2.ToString());
        levelDesc.TargetText3.text = data.Element("target3").Value.Replace("NUMBER", GameRule.target3.ToString());

        levelInformation.SetActive(true);
        backBtn.SetActive(false);
        MenuManager.level = level;
    }

    // Загрузка игры
    public void loadGame()
    {
        AudioManager.PlaySound(AudioManager.Audio.click);
        LocalizationManager.clear();
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
            StartCoroutine(scalePlanet(planet, true));

            for (int i = 2; i < galaxy.transform.childCount; ++i)
                if (galaxy.transform.GetChild(i).transform.gameObject != planet.transform.gameObject)
                    StartCoroutine(fadePlanet(i, true));

            Button btn = planet.GetComponent<Button>();
            btn.enabled = !btn.enabled;
            thisCamera.GetComponent<UIParallax>().setActive(false);

            AudioManager.PlaySound(AudioManager.Audio.select);
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

        Button btn = planetTmp.GetComponent<Button>();
        btn.enabled = !btn.enabled;

        thisCamera.GetComponent<UIParallax>().setActive(true);

        AudioManager.PlaySound(AudioManager.Audio.select);
    }

    // Анимация прорисовки уровней
    IEnumerator Spawn(GameObject planet, int num)
    {
        float timeDelay;

        //пробегаемся по всем уровням планеты (planet)
        for (int i = 0; i < planet.transform.childCount; ++i)
        {
            //получем обьект уровня (ChildLvl) и получаем прогресс игрока
            GameObject ChildLvl = planet.transform.GetChild(i).gameObject;
            byte progress = PlayerData.getInstance().progress[num].first[i];

            //пробегаемся по обьектам уровня level,point....
            for (int j = 0; j < ChildLvl.transform.childCount; ++j)
            {
                //задержка
                timeDelay = 0.1f;
                
                //элементы уровня
                GameObject child = ChildLvl.transform.GetChild(j).gameObject;

                //в случае если в прогрессе не 0 звезд
                if (progress != 0)
                {
                    //если это planet_Level,то ставим звезды относительно прогресса
                    if (j == 0)
                        for (int k = 1; k <= progress; ++k)
                            child.transform.GetChild(k).gameObject.SetActive(true);
                    child.gameObject.SetActive(true);
                }
                else
                {
                    //есои это левел и если это 1 уровень планеты или прогресс игрока  не равен 0 звездам
                    if (j == 0 && (i == 0 || PlayerData.getInstance().progress[num].first[i - 1] != 0))
                        child.gameObject.SetActive(true);
                    //если это левел
                    else if (j == 0)
                    {
                        //делаем затемненой кнопку
                        child.GetComponent<Image>().color = new Color32(204, 204, 204, 255);
                        //выключаем возможность нажатия
                        Button btn = child.GetComponent<Button>();
                        btn.enabled = false;
                        child.gameObject.SetActive(true);
                    }
                    else
                    {
                        //выключаем ?
                        child.gameObject.SetActive(true);
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
    IEnumerator scalePlanet(GameObject planet, bool to)
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
        Transform planet = galaxy.transform.GetChild(num).transform;
        Color color = planet.GetComponent<Image>().color;
        Color[] planetChild = new Color[planet.childCount];

        for(int i = 0; i < planet.childCount; ++i)
            planetChild[i] = planet.GetChild(i).GetComponent<Image>().color;
        

        if (to)
        {
            while (galaxy.transform.GetChild(num).transform.GetComponent<Image>().color.a > 0)
            {
                float time = Time.deltaTime / 1;

                //изменение альфа канала планеты
                color.a -= time;
                planet.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);

                //изменение альфа канала обьектов планеты
                for (int i = 0; i < planet.childCount; ++i)
                {
                    planetChild[i].a -= time;
                    planet.GetChild(i).GetComponent<Image>().color = new Color(planetChild[i].r, planetChild[i].g, planetChild[i].b, planetChild[i].a);
                }
                yield return null;
            }
        }
        else
        {
            while (galaxy.transform.GetChild(num).transform.GetComponent<Image>().color.a < 1)
            {
                float time = Time.deltaTime / 1;

                //изменение альфа канала планеты
                color.a += time;
                planet.GetComponent<Image>().color = new Color(color.r, color.g, color.b, color.a);

                //изменение альфа канала обьектов планеты
                for (int i = 0; i < planet.childCount; ++i)
                {
                    planetChild[i].a += time;
                    planet.GetChild(i).GetComponent<Image>().color = new Color(planetChild[i].r, planetChild[i].g, planetChild[i].b, planetChild[i].a);
                }
                yield return null;
            }
        }
    }

    //устанавливаем прогресс планет
    public void setPlanetProgress()
    {
        Color gray = new Color32(126, 126, 126, 255);

        for (int i = 0; i < MenuManager.allPlanets; ++i)
        {
            Transform planet = galaxy.transform.GetChild(i + 2);

            if (!PlayerData.getInstance().progress[i].second.second)
            {
                planet.GetChild(0).gameObject.SetActive(true);
                planet.GetComponent<Image>().color = gray;
                planet.GetComponent<Button>().enabled = false;
            }
        }
    }
}

