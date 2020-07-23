using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml.Linq;
using UnityEngine.SceneManagement;
using BaseStructures;

/* Класс для работы с локализацией текста
 * Статический класс
 */
public static class LocalizationManager
{
    private static List<Pair<Text, string>> texts = new List<Pair<Text, string>>();
    private static XElement data; // Данные XML файла

    // Добавление текста в список текста который необходимо локализовать
    public static void add(Pair<Text, string> text)
    {
        if (data == null)
            loadXML();
        texts.Add(text);
        text.first.text = data.Element(text.second).Value;
    }

    // Загрузка данных из XML файла
    private static void loadXML()
    {
        TextAsset textAsset = (TextAsset) Resources.Load("XML/Lozalization/" + PlayerData.getInstance().lang.ToString() + "/" + SceneManager.GetActiveScene().name);
        data = XDocument.Parse(textAsset.text).Element("localization");
    }

    // Установка языка
    public static void resetLanguage()
    {
        loadXML();
        for (int i = 0; i < texts.Count; ++i)
            texts[i].first.text = data.Element(texts[i].second).Value;
    }    
}
