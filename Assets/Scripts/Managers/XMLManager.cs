using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;

/* Класс для работы с данными
 * Singleton
 */
public class XMLManager
{
    public static XMLManager instance = null;

    private XMLManager()
    { }

    public static XMLManager getInstance()
    {
        if (instance == null)
            instance = new XMLManager();
        return instance;
    }

    public Difficulty difficulty; 

    public void SaveItems(string mode,float speedAI, float accuracyAI,float timeRest)
    {
        difficulty.mode = mode;
        difficulty.speedAI = speedAI;
        difficulty.accuracyAI = accuracyAI;
        difficulty.timeRest = timeRest;

        XmlSerializer serializer = new XmlSerializer(typeof(Difficulty));
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/settings.xml", FileMode.Create);
        serializer.Serialize(fileStream, difficulty);
        fileStream.Close();
    }

    public void LoadItems()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Difficulty));
        try
        {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/settings.xml", FileMode.Open);
            difficulty = (Difficulty)serializer.Deserialize(fileStream);
            fileStream.Close();
        }
        catch
        {
            SaveItems(Difficulties.Begginer.mode, Difficulties.Begginer.speed, Difficulties.Begginer.accuracyAI, Difficulties.Begginer.timeRest);
            LoadItems();
        }
    }


    /* Универсальная сериализация
     * Параметры:
     * data - класс который необходимо сериализовать, name - имя xml файла (желательно имя самого класса, чтобы не забыть)
     */
    public static void SaveData<T>(T data, string name)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream fileStream = new FileStream(@"D:\GAMES\Unity\Projects\FastSlingpuck\Assets\Resources\" + name + ".xml", FileMode.Create);
        //FileStream fileStream = new FileStream(Application.persistentDataPath + '/' + name + ".xml", FileMode.Create);
        serializer.Serialize(fileStream, data);
        fileStream.Close();
    }

    /* Универсальная десериализация
     * Параметры:
     * data - класс который необходимо десериализовать, name - имя xml файла (желательно имя самого класса, чтобы не забыть)
     */
    public static bool LoadData<T>(ref T data, string name)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        try
        {
            FileStream fileStream = new FileStream(@"D:\GAMES\Unity\Projects\FastSlingpuck\Assets\Resources\" + name + ".xml", FileMode.Open); // Дебаг, для комп
            //FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerData.xml", FileMode.Open);     
            data = (T)(serializer.Deserialize(fileStream));
            fileStream.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

}

[System.Serializable]
public class Difficulty
{
    public string mode;
    public float speedAI;
    public float accuracyAI;
    public float timeRest;
}

