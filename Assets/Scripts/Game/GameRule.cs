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
        jungle
    }
    public enum Difficulties
    {
        begginer,
        skilled,
        master,
        god,
        chinese
    }

    public enum AI
    {
        AI, 
        RotationAI
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
    public static bool ActiveAI; // Наличие ИИ
    public static Difficulties difficulties; // Сложность игры

    public static int planetNum, levelNum, levelsCount; // номер планеты, номер уровня и кол-во уровней планеты, необходимы для записи прогреса
    public static int target1, target2, target3; // Цели, как правило это всегда числовые значение, если это касается особенности геймплея, то это обеспечивает сам режим

    public static Type[] planetProgressNum = new Type[4] { Type.lava, Type.sand, Type.ice, Type.jungle };
}

public class Difficulty
{
    public string mode;
    public float speedAI;
    public float accuracyAI;
    public float timeRest;
}
