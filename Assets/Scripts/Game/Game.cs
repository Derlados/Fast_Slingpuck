using System;
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

    public bool activeGame = false;

    private void Start()
    {
        countShots = 0;

        mode = GameRule.mode;
        type = GameRule.type;

        GameObject checkers = null;
        PlayerData playerData = PlayerData.getInstance();

        // Модификация компонентов относительно выбраных настроек
        switch (mode)
        {
            case GameRule.Mode.Normal:              
                window.AddComponent<NormalWindow>();
                for (int i = 0; i < checkersNormal.transform.childCount; ++i)
                    checkersNormal.transform.GetChild(i).gameObject.AddComponent<Modifier>();
                 checkers = checkersNormal;
                break;
            case GameRule.Mode.Speed:
                window.AddComponent<DestroyWindow>();
                for (int i = 0; i < checkersSpeed.transform.childCount; ++i)
                    checkersSpeed.transform.GetChild(i).gameObject.AddComponent<Destroy>();
                checkers = checkersSpeed;
                break;
        }

        //изменение спрайтов чекеров игрока
        for (int i = 0; i < checkers.transform.childCount / 2; ++i)
        {
            Image userImg = checkers.transform.GetChild(i).gameObject.transform.GetComponent<Image>();
            userImg.sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + playerData.puckSprite);
            userImg.material = Resources.Load<Material>("Sprites/Materials/Checker/" + playerData.puckSprite + "_glowMat");
        }

        
        gameObject.AddComponent(Type.GetType(GameRule.mode.ToString()));
        gate.AddComponent(Type.GetType(GameRule.typeGate.ToString()));

        if (GameRule.ActiveAI)
        {
            AI.AddComponent(Type.GetType(GameRule.TypeAI.ToString()));
            AI.SetActive(true);
        }

        // Наложение соответствующий текстур
        ChangePlanetSprite(type.ToString() + "_planet");
        if (GameRule.ActiveAI)
            ChangeCheckerSprite(type.ToString() + "_CheckerGlowMat", checkers);
        ChangeParticle(type.ToString() + "_particle",true);

        //установка небходимой музыки
        if(GameRule.levelNum == GameRule.levelsCount)
            backgroundMusic.transform.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Game/songs/boss");
        else
            backgroundMusic.transform.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("Audio/Game/songs/" + GameRule.type.ToString() + "_level");

        backgroundMusic.transform.GetComponent<AudioSource>().Play();
    }

    // Установка спрайтов поля и шайб
    void ChangePlanetSprite(string spriteName)
    {
        imgField.sprite = Resources.Load<Sprite>("Sprites/levels/planets/" + spriteName);
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

    public void ChangeParticle(string particleName,bool active)
    {
        foreach(Transform t in particles.transform)
        {
            if (t.name == particleName)
                t.gameObject.SetActive(active);
        }
    }
}
