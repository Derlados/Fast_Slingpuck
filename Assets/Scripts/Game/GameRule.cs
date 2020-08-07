using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Статический класс настроек игры, необходим для передачи данных между сценами

public static class GameRule
{
    public enum Mode
    {
        Normal,
        Speed
    }
    public enum Type
    {
        lava,
        ice,
        sand,
        jungle,
        water
    }
    public enum Difficulties : byte
    {
        begginer = 0,
        skilled = 1,
        master = 2,
        god = 3,
        chinese = 4
    }

    public enum AI
    {
        None,
        AI, 
        RotationAI
    }

    public enum Gate
    {
        Gate,
        MovementGate,
        TeleportGate,
        RandomMovementGate,
    }

    public enum CheckerModifier
    {
        Heavy
    }

    /* Настройки уровня
     * mode - режим игры
     * type - тип поля
     * Ai - наличие бота. true - игра с ботом, false - игра без бота
     * difficulties - сложность игры
     */
    public static Mode mode; // Режим игры
    public static Type type; // Тип планеты
    public static AI TypeAI; // Вид ИИ
    public static Gate typeGate; // Вид ворот
    public static Difficulties difficulties; // Сложность игры
    public static List<int> levelsCount = new List<int>();

    public static int planetNum, levelNum, levelCount; // номер планеты, номер уровня и кол-во уровней планеты, необходимы для записи прогреса
    public static int target1, target2, target3; // Цели, как правило это всегда числовые значение, если это касается особенности геймплея, то это обеспечивает сам режим

    // Модификаторы шайб на уровни
    public static List<CheckerModifier> AIModifier = new List<CheckerModifier>();
    public static List<CheckerModifier> GlobalModifier = new List<CheckerModifier>();

    //конвертация numberPlanet в GameRule.Type
    public static Type[] planetProgressNum = new Type[] { Type.lava, Type.ice, Type.sand,  Type.water, Type.jungle, };

    public static bool globalDiff = false; // false - сложность обычная, true - повышенная сложность для всех уровней
}

public class Difficulty
{
    public string mode;
    public float speedAI;
    public float accuracyAI;
    public float timeRest;
}
