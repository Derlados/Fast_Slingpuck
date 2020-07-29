using UnityEngine;

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
    public int numLevel; // Номер уровня, необходимо знать для того чтобы в дальнейшем записать результат

    // Установка всех игровых правил и запус игры
    public void setGameRule(GameObject planet)
    {
        GameRule.mode = mode;
        GameRule.type = type;
        GameRule.typeGate = typeGate;
        GameRule.difficulties = difficulties;
        GameRule.levelNum = numLevel;
        GameRule.levelsCount = planet.transform.childCount-1;
        GameRule.TypeAI = typeAI;

        CheckerModifiers checkerModifiers = GetComponent<CheckerModifiers>();
        for (int i = 0; i < checkerModifiers.AIModifier.Count; ++i)
            GameRule.AIModifier.Add(checkerModifiers.AIModifier[i]);

        setTargets();

        GameObject.FindGameObjectWithTag("MenuManager").GetComponent<MenuManager>().loadLevelDesc(this);
    }

    public void setTargets()
    {
        AudioManager.PlaySound(AudioManager.Audio.select);
        switch (GameRule.mode)
        {
            case GameRule.Mode.Normal:
                GameRule.target2 = normal.time;
                GameRule.target3 = normal.countCheckers;
                break;
            case GameRule.Mode.Speed:
                GameRule.target1 = speed.minTargetCheckers;
                GameRule.target2 = speed.maxTargetCheckers;
                GameRule.target3 = speed.accuracy;
                break;
        }
    }
}
