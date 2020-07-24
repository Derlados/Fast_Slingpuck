using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    public string id;

    void Awake()
    {
        LocalizationManager.add(new Pair<Text, string>(gameObject.GetComponent<Text>(), id));
    }
}
