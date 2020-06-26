using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    LocalizationManager locManager;

    public string id;

    // Start is called before the first frame update
    void Awake()
    {
        locManager = LocalizationManager.getInstance();
        locManager.add(new Pair<Text, string> (gameObject.GetComponent<Text>(), id));
    }
}
