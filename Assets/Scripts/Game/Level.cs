using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelData
{
    //normal mode
    public int time;
    public int countCheckers;

    //speed mode
    public int minTargetCheckers;
    public int maxTargetCheckers;
    public int accuracy;

    //general 
    public GameRule.Mode mode; // Режим игры
    public GameRule.Type type; // Тип планеты
    public GameRule.AI typeAI;
    public GameRule.Gate typeGate;
    public GameRule.Difficulties difficulties; // Сложность игры
    public int numLevel; // Номер уровня, необходимо знать для того чтобы в дальнейшем записать результат

    public List<GameRule.CheckerModifier> AIModifier = new List <GameRule.CheckerModifier>();
}

public class Level : MonoBehaviour
{
    // Цели для режима Normal
    [System.Serializable]
    public class Normal
    {
        public int time;
        public int countCheckers;
    }
    public Normal normal; 

    // Цели для режима Speed
    [System.Serializable]
    public class Speed
    {
        public int minTargetCheckers; [Tooltip("If player will play without AI")]
        public int maxTargetCheckers;
        public int accuracy;
    }
    public Speed speed;

    public GameRule.Mode mode; // Режим игры
    public GameRule.Type type; // Тип планеты
    public GameRule.AI typeAI;
    public GameRule.Gate typeGate;
    public GameRule.Difficulties difficulties; // Сложность игры
    //public int numLevel; // Номер уровня, необходимо знать для того чтобы в дальнейшем записать результат

    static LevelData levelData = new LevelData();

    // Установка всех игровых правил и запус игры
    public static void setGameRule(bool next, int numberPlanet, int numLevel)
    {
        levelData.type = GameRule.planetProgressNum[numberPlanet];
        levelData.numLevel = numLevel;

        XMLManager.LoadLevel(ref levelData, levelData.type.ToString(), levelData.numLevel);

        GameRule.mode = levelData.mode;
        GameRule.type = levelData.type;
        GameRule.TypeAI = levelData.typeAI;
        GameRule.typeGate = levelData.typeGate;
        GameRule.difficulties = levelData.difficulties;
        GameRule.levelNum = levelData.numLevel;
        GameRule.levelCount = GameRule.levelsCount[0];

        for (int i = 0; i < levelData.AIModifier.Count; ++i)
            GameRule.AIModifier.Add(levelData.AIModifier[i]);

        setTargets();
        if (!next)
        {
            GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>().loadLevelDesc(levelData);
        }
        else
        {
            MenuManager.level = levelData;
            SceneManager.LoadScene("Game");
        }
    }

    public static void setTargets()
    {
        AudioManager.PlaySound(AudioManager.Audio.select);
        switch (GameRule.mode)
        {
            case GameRule.Mode.Normal: 
                GameRule.target2 = levelData.time;
                GameRule.target3 = levelData.countCheckers;
                break;
            case GameRule.Mode.Speed:
                GameRule.target1 = levelData.minTargetCheckers;
                GameRule.target2 = levelData.maxTargetCheckers;
                GameRule.target3 = levelData.accuracy;
                break;
        }
    }
}
