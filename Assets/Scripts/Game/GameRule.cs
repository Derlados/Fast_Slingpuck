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

    public static Mode mode;
    public static Type type;
    public static bool AI = false; 
}
