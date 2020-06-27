using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using BaseStructures;

/* Класс для работы с локализацией текста
 * Singleton
 */
public class LocalizationManager : MonoBehaviour
{
    private List<Pair<Text, string>> texts = new List<Pair<Text, string>>();
    public static LocalizationManager instance = null;
    
    XElement data; // Данные XML файла

    // Набор языков
    public enum language : byte
    {
        RU,
        EN,
        UA
    }
    public language curLanguage = language.RU; // Текущий язык

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            loadXML();
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Добавление текста в список текста который необходимо локализовать
    public void add(Pair<Text, string> text)
    {
        texts.Add(text);
        text.first.text = data.Element(text.second).Value;
    }

    // Загрузка данных из XML файла
    private void loadXML()
    {
        TextAsset textAsset = (TextAsset) Resources.Load(curLanguage.ToString() + "/" + SceneManager.GetActiveScene().name);
        data = XDocument.Parse(textAsset.text).Element("localization");
    }

    // Установка языка
    public void resetLanguage()
    {
        loadXML();
        for (int i = 0; i < texts.Count; ++i)
            texts[i].first.text = data.Element(texts[i].second).Value;
    }    

    public void clear()
    {
        texts.Clear();
    }
}
