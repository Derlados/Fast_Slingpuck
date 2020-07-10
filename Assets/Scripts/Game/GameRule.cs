using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameRule
{
    public enum Mode
    {
        normal,
        speed
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

    /* Настройки уровня
     * mode - режим игры
     * type - тип поля
     * Ai - наличие бота. true - игра с ботом, false - игра без бота
     * difficulties - сложность игры
     */
    public static Mode mode; 
    public static Type type;
    public static bool AI = false; 
    public static Difficulties difficulties;

}
