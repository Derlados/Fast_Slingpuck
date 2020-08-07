using GooglePlayGames.BasicApi.Multiplayer;
using System;
using System.Globalization;
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

    void ChangeAICheckerSprite(string planetName, string spriteName, GameObject checkers)
    {
        for (int i = checkers.transform.childCount / 2; i < checkers.transform.childCount; ++i)
        {
            checkers.transform.GetChild(i).gameObject.SetActive(true);

            //установка спрайта
            Image img = checkers.transform.GetChild(i).gameObject.GetComponent<Image>();
            img.sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + spriteName);

            //установка материала
            switch (mode)
            {
                case GameRule.Mode.Normal:
                    img.material = Resources.Load<Material>("Sprites/Materials/Checker/AI_CheckerGlowMat");
                    break;
                case GameRule.Mode.Speed:
                    img.material = Resources.Load<Material>("Sprites/Materials/Checker/AI_CheckerDissolveMat");
                    break;
            }

            //установка текстуры материала
            img.material.SetTexture("_MainTex", Resources.Load<Texture>("Sprites/levels/checkers/" + spriteName));
            img.material.SetTexture("_Emission", Resources.Load<Texture>("Sprites/Materials/Checker/" + planetName + "_GlowPartOfChecker"));

            //установка цвета материала
            TextAsset textAsset = (TextAsset)Resources.Load("XML/Game/checkerColors");
            XElement xdoc = XDocument.Parse(textAsset.text).Element("checkerColors");

            int red = 0, green = 0, blue = 0;
            float intensity = 1f;

            foreach (XElement clr in xdoc.Elements(spriteName))
            {
                red = int.Parse(clr.Element("red").Value);
                green = int.Parse(clr.Element("green").Value);
                blue = int.Parse(clr.Element("blue").Value);
                intensity = float.Parse(clr.Element("intensity").Value, CultureInfo.InvariantCulture);
            }

            float factor = (float)Math.Exp(-5.5115115f + 0.68895733f * intensity);

            Color color = new Color(red * factor, green * factor, blue * factor);
            img.material.SetColor("_Color", color);

            //установка градиента цвета трея
            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            //_Color - id цвета в материале чекеров AI
            colorKey = new GradientColorKey[2];
            colorKey[0].color = img.material.GetColor("_Color");
            colorKey[0].time = 0.0f;
            colorKey[1].color = img.material.GetColor("_Color");
            colorKey[1].time = 1.0f;

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


    void ChangeCheckerSprite(string spriteName, GameObject checkers)
    {
        for (int i = 0; i < checkers.transform.childCount / 2; ++i)
        {
            //установка спрайта 
            Image userImg = checkers.transform.GetChild(i).gameObject.GetComponent<Image>();
            userImg.sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + spriteName);

            //установка материала
            switch (mode)
            {
                case GameRule.Mode.Normal:
                    userImg.material = Resources.Load<Material>("Sprites/Materials/Checker/user_CheckerGlowMat");
                    break;
                case GameRule.Mode.Speed:
                    userImg.material = Resources.Load<Material>("Sprites/Materials/Checker/user_DissolveMat");
                    break;
            }

            //установка текстуры материала
            Texture texture = Resources.Load<Texture>("Sprites/levels/checkers/" + spriteName);
            userImg.material.SetTexture("_MainTex", texture);

            //установка цвета материала
            TextAsset textAsset = (TextAsset)Resources.Load("XML/Game/checkerColors");
            XElement xdoc = XDocument.Parse(textAsset.text).Element("checkerColors");

            int red=0, green = 0, blue = 0;
            float intensity = 1f;

            foreach (XElement clr in xdoc.Elements(spriteName))
            {
                 red = int.Parse(clr.Element("red").Value);
                 green = int.Parse(clr.Element("green").Value);
                 blue = int.Parse(clr.Element("blue").Value);
                 intensity = float.Parse(clr.Element("intensity").Value, CultureInfo.InvariantCulture);
            }

            float factor = (float)Math.Exp(-5.5115115f + 0.68895733f * intensity);

            Color color = new Color(red * factor, green * factor, blue * factor);
            userImg.material.SetColor("_Color", color);

            //установка градиента цвета трея
            Gradient gradient;
            GradientColorKey[] colorKey;
            GradientAlphaKey[] alphaKey;

            //_Color - id цвета в материале чекеров 
            colorKey = new GradientColorKey[2];
            colorKey[0].color = userImg.material.GetColor("_Color");
            colorKey[0].time = 0.0f;
            colorKey[1].color = userImg.material.GetColor("_Color");
            colorKey[1].time = 1.0f;

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
        AudioSource audioSource = backgroundMusic.transform.GetComponent<AudioSource>();
        if (GameRule.levelNum == (GameRule.levelCount - 1))
            audioSource.clip = Resources.Load<AudioClip>("Audio/Game/songs/boss");
        else
            audioSource.clip = Resources.Load<AudioClip>("Audio/Game/songs/" + type.ToString() + "_level");

        audioSource.volume = PlayerData.getInstance().volume;
        audioSource.Play();
    }

    public void loadSprites()
    {
        GameObject checkers = initGameRule();

        //изменение спрайтов чекеров игрока
        ChangeCheckerSprite(PlayerData.getInstance().puckSprite, checkers);

        // Наложение соответствующий текстур
        ChangePlanetSprite(type.ToString() + "_planet");

        //установка спрайтов чекеров бота
        if (GameRule.TypeAI != GameRule.AI.None)
            ChangeAICheckerSprite(type.ToString(), type.ToString() + "_checker", checkers);

        //установка частиц
        ChangeParticle(type.ToString() + "_particle", true);
    }
}

