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

    public static Mode mode;
    public static Type type;
    public static bool AI = false; 
    public static Difficulties difficulties;

}
