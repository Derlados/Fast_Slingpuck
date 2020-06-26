using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using BaseStructures;

/* Класс для работы с локализацией текста
 * Singleton
 */
public class LocalizationManager
{
    static List<Pair<Text, string>> texts = new List<Pair<Text, string>>();
    static LocalizationManager instance = null;
    
    XElement data; // Данные XML файла

    // Набор языков
    enum language : byte
    {
        RU,
        EU,
        UA
    }
    language curLanguage = language.UA; // Текущий язык

    private LocalizationManager()
    { }
    public static LocalizationManager getInstance()
    {
        if (instance == null)
            instance = new LocalizationManager();
        return instance;
    }

    // Добавление текста в список текста который необходимо локализовать
    public void add(Pair<Text, string> text)
    {
        texts.Add(text);
    }

    // Инициализация (Загрузка XML файла и установка языка)
    public void Init()
    {
        loadXML();
        setLanguage();
    }

    // Загрузка данных из XML файла
    private void loadXML()
    {
        TextAsset textAsset = (TextAsset) Resources.Load(curLanguage.ToString() + "/" + SceneManager.GetActiveScene().name);
        data = XDocument.Parse(textAsset.text).Element("localization");
    }

    // Установка языка
    private void setLanguage()
    {
        for (int i = 0; i < texts.Count; ++i)
        {
            texts[i].first.text = data.Element(texts[i].second).Value;
        }
    }    
}
