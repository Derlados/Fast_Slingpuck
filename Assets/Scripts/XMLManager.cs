using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

/* Класс для работы с данными
 * Singleton
 */
public class XMLManager
{
    public static XMLManager instance = null;

    public static XMLManager getInstance()
    {
        if (instance == null)
            instance = new XMLManager();
        return instance;
    }

    private XMLManager()
    { }

    public Difficulty difficulty; 
    public Player player;

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

    public void SavePlayer(int score)
    {
        LoadPlayer();
        player.score += score;

        XmlSerializer serializer = new XmlSerializer(typeof(Player));
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/player.xml", FileMode.Create);
        serializer.Serialize(fileStream, player);
        fileStream.Close();
    }

    public void LoadPlayer()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Player));
        try
        {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/player.xml", FileMode.Open);
            player = (Player)serializer.Deserialize(fileStream);
            fileStream.Close();
        }
        catch
        {
            player.score = 0;
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/player.xml", FileMode.Create);
            serializer.Serialize(fileStream, player);
            fileStream.Close();
            LoadPlayer();
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

[System.Serializable]
public class Player
{
    public int score;
}


