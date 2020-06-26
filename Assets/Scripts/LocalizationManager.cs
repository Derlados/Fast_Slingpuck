using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using BaseStructures;

public class LocalizationManager : MonoBehaviour
{
    static List<Pair<Text, string>> texts;
    static LocalizationManager instance = null;
    
    XElement data; // Данные XML файла
    public string curLanguage; // Текущий язык

    public Text temp;

    private LocalizationManager()
    { }
    public static LocalizationManager getInstance()
    {
        if (instance == null)
            instance = new LocalizationManager();
        return instance;
    }

    public void add(Pair<Text, string> text)
    {
        texts.Add(text);
    }

    private void Start()
    {
        loadXML();
        setLanguage();
    }

    private void loadXML()
    {
        Debug.Log(curLanguage + "/" + SceneManager.GetActiveScene().name);
        TextAsset textAsset = (TextAsset) Resources.Load(curLanguage + "/" + SceneManager.GetActiveScene().name);
        data = XDocument.Parse(textAsset.text).Element("localization");
    }

    private void setLanguage()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            texts[i].first.text = data.Element(texts[i].second).Value;
        }
    }    
}
