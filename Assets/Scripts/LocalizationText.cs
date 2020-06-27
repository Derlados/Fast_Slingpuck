using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalizationText : MonoBehaviour
{
    public string id;

    // Start is called before the first frame update
    void Start()
    {
        LocalizationManager.instance.add(new Pair<Text, string>(gameObject.GetComponent<Text>(), id));
    }
}
