using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

// Класс отвечающий за геймплей в самой игре
public class Game : MonoBehaviour
{
    /* gameOverText - текст с сообщение об окончании игры
     * upCountText - текст показывающий счет верхнего игрока
     * downCountText - текст показывающий счет нижнего игрока
     * gameStartCounterText - текст показывающий отсчет до начала игры
     * scoreText - текст показывающий набранные очки в игре
     */
    public GameObject AI;
    public GameObject capperField;
    public GameObject downBorderHolder, upBorderHolder, window;
    public GameObject checkersNormal, checkersSpeed;
    public GameObject gameMenu;
    public GameObject particles;
    public GameObject gate;
    public GameObject borders;
    public GameObject[] strings;
    public GameObject MainCamera;

    // Текст счетчиков
    public Text upCountText, downCountText, gameCounter;
    //картинка фишки из быстрого режима
    public Image imgField;

    public GameObject backgroundMusic;

    GameRule.Mode mode; // Режим игры
    GameRule.Type type; // Тип карты (текстуры)

    // Количество звезд которые получит игрок пройдя уровень
    public byte countStars = 3;

    // Действия игрока
    public static int countShots;

    public static bool activeGame = false;

    private void Awake()
    {
        countShots = 0;

        mode = GameRule.mode;
        type = GameRule.type;

        loadNeon(type.ToString());
        loadSprites();
        loadMusic();
    }

    // Устанавливает все необходимые настройки для уровня и возвращает активные шайбы
    private GameObject initGameRule()
    {
        GameObject checkers = null;

        // Модификация компонентов относительно выбраных настроек
        switch (mode)
        {
            case GameRule.Mode.Normal:
                checkers = checkersNormal;
                for (int i = 0; i < checkers.transform.childCount; ++i)
                    checkers.transform.GetChild(i).gameObject.AddComponent<Modifier>();
                window.AddComponent<NormalWindow>();
                break;
            case GameRule.Mode.Speed:
                checkers = checkersSpeed;
                for (int i = 0; i < checkers.transform.childCount; ++i)
                    checkers.transform.GetChild(i).gameObject.AddComponent<Destroy>();
                window.AddComponent<DestroyWindow>();      
                break;
        }

        // Устанавливаем модификаторы шайб для бота и свойства поля (планеты)
        for (int i = checkers.transform.childCount / 2; i < checkers.transform.childCount; ++i)
            for (int j = 0; j < GameRule.AIModifier.Count; ++j)
                checkers.transform.GetChild(i).gameObject.AddComponent(Type.GetType(GameRule.AIModifier[j].ToString()));

        // Добавляем пользователю возможность поворачивать шайбу, если он такую возможность выбрал
        if (PlayerData.getInstance().checkerRotation)
            for (int i = 0; i < checkers.transform.childCount; ++i)
                checkers.transform.GetChild(i).gameObject.AddComponent<Rotation>();

        gameObject.AddComponent(Type.GetType(GameRule.type.ToString()));
        gameObject.GetComponent<Field>().setGlobalModififers(checkers);

        // Добавляем режим, тип ворот и тип бота
        gameObject.AddComponent(Type.GetType(mode.ToString()));
        gate.AddComponent(Type.GetType(GameRule.typeGate.ToString()));
        if (GameRule.TypeAI != GameRule.AI.None)
        {
            AI.AddComponent(Type.GetType(GameRule.TypeAI.ToString()));
            AI.SetActive(true);
        }

        return checkers;
    }

    // Установка спрайта поля
    void ChangePlanetSprite(string spriteName)
    {
        imgField.sprite = Resources.Load<Sprite>("Sprites/levels/fields/" + spriteName);
        if(type.ToString() == "water") imgField.color = new Color32(188, 188, 188, 255);
    }

    void ChangeCheckerSprite(string matName, GameObject checkers)
    {
        for (int i = checkers.transform.childCount / 2; i < checkers.transform.childCount; ++i)
        {
            checkers.transform.GetChild(i).gameObject.SetActive(true);
            Image img = checkers.transform.GetChild(i).gameObject.GetComponent<Image>();
            img.material = Resources.Load<Material>("Sprites/Materials/Checker/" + matName);

            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[2];
            colorKey[0].color = img.material.GetColor("Color_35045387");
            colorKey[0].time = 0.0f;
            colorKey[1].color = img.material.GetColor("Color_35045387");
            colorKey[1].time = 1.0f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 0.2f;
            alphaKey[1].time = 1.0f;

            gradient = new Gradient();
            gradient.SetKeys(colorKey, alphaKey);

            TrailRenderer trailRenderer = checkers.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject.GetComponent<TrailRenderer>();
            trailRenderer.colorGradient = gradient;
        }
    }

    public void ChangeParticle(string particleName, bool active)
    {
        foreach (Transform t in particles.transform)
        {
            if (t.name == particleName)
                t.gameObject.SetActive(active);
        }
    }

    //загрузка цвета стенок
    public void loadNeon(string key)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Game/levelData");
        XElement xdoc = XDocument.Parse(textAsset.text).Element("levelData");

        foreach (XElement diff in xdoc.Elements(key))
        {
            //стенки ворот
            for (int i = 1; i <= 2; ++i)
                gate.transform.GetChild(i).GetComponent<Image>().material = Resources.Load<Material>("Sprites/Materials/Borders/" + diff.Element("gate").Value.ToString());

            //верхняя и нижняя стенки
            for (int i = 0; i < 2; ++i)
                borders.transform.GetChild(i).GetComponent<Image>().material = Resources.Load<Material>("Sprites/Materials/Borders/" + diff.Element("topBorders").Value.ToString());
            
            //левая и правая стенки
            for (int i = 2; i < 4; ++i)
                borders.transform.GetChild(i).GetComponent<Image>().material = Resources.Load<Material>("Sprites/Materials/Borders/" + diff.Element("sideBorders").Value.ToString());

            //верхняя и нижняя нитка
            for (int i = 0; i < 2; ++i)
                strings[i].transform.GetComponent<LineRenderer>().material = Resources.Load<Material>("Sprites/Materials/Borders/" + diff.Element("string").Value.ToString());
        }

        ScreenOptimization.setNeonRadius(MainCamera);

    }

    //установка небходимой музыки
    public void loadMusic()
    {
        if (GameRule.levelNum == GameRule.levelsCount)
            backgroundMusic.transform.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Game/songs/boss");
        else
            backgroundMusic.transform.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Game/songs/" + type.ToString() + "_level");

        backgroundMusic.transform.GetComponent<AudioSource>().Play();
    }

    public void loadSprites()
    {
        GameObject checkers = initGameRule();

        //изменение спрайтов чекеров игрока
        for (int i = 0; i < checkers.transform.childCount / 2; ++i)
        {
            Image userImg = checkers.transform.GetChild(i).gameObject.transform.GetComponent<Image>();
            userImg.sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + PlayerData.getInstance().puckSprite);
            // userImg.material = Resources.Load<Material>("Sprites/Materials/Checker/" + playerData.puckSprite + "_glowMat");
        }

        // Наложение соответствующий текстур
        ChangePlanetSprite(type.ToString() + "_planet");
        if (GameRule.TypeAI != GameRule.AI.None)
            ChangeCheckerSprite(type.ToString() + "_CheckerGlowMat", checkers);
        ChangeParticle(type.ToString() + "_particle", true);
    }
}

