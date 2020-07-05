using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public GameRule.Mode mode;
    public GameRule.Type type;

    void setRules()
    {
        GameRule.mode = mode;
        GameRule.type = type;
    }
}
