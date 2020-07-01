using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Xml.Linq;
using System.Globalization;

public class XMLManager
{
    /* Универсальная сериализация
     * Параметры:
     * data - класс который необходимо десериализовать 
     * name - имя xml файла (желательно имя самого класса, чтобы не забыть)
     */
    public static void SaveData<T>(T data, string name)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream fileStream = new FileStream(Application.dataPath + "/Data/" + name + ".xml", FileMode.Create);
        //FileStream fileStream = new FileStream(Application.persistentDataPath + '/' + name + ".xml", FileMode.Create);
        serializer.Serialize(fileStream, data);
        fileStream.Close();
    }

    /* Универсальная десериализация
     * Параметры:
     * data - класс который необходимо сериализовать 
     * name - имя xml файла 
     */
    public static void LoadData<T>(ref T data, string name)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream fileStream = new FileStream(Application.dataPath + "/Data/" + name + ".xml", FileMode.Open);
        //FileStream fileStream = new FileStream(Application.persistentDataPath + '/' + name + ".xml", FileMode.Create);
        data = (T)serializer.Deserialize(fileStream);
        fileStream.Close();
    }

    /* Универсальная загрузка сложностей
     * Параметры:
     * data - класс который необходимо сериализовать 
     * name - имя xml файла 
     * key - уровень сложности
     */
    public static void LoadDifficulty(ref Difficulty data, string key)
    {
        FileStream fileStream = new FileStream(Application.dataPath + "/Resources/Settings/Difficulties" + ".xml", FileMode.Open);
        //FileStream fileStream = new FileStream(Application.persistentDataPath + '/' + name + ".xml", FileMode.Create);
        XDocument xdoc = XDocument.Load(fileStream);
        foreach (XElement diff in xdoc.Element("difficulties").Elements(key))
        {
            data.mode = diff.Element("mode").Value;
            data.speedAI = float.Parse(diff.Element("speedAI").Value);
            data.accuracyAI = float.Parse(diff.Element("accuracyAI").Value, CultureInfo.InvariantCulture);
            data.timeRest = float.Parse(diff.Element("timeRest").Value, CultureInfo.InvariantCulture);
        }
        fileStream.Close();
    }
}

