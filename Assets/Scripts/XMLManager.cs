using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager ins;

    private void Awake()
    {
        ins = this;
    }

    public Difficulty difficulty;

    public void SaveItems(string mode,float speedAI, float accuracyAI)
    {
        difficulty.mode = mode;
        difficulty.speedAI = speedAI;
        difficulty.accuracyAI = accuracyAI;

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
            SaveItems(Begginer.mode, Begginer.speed, Begginer.accuracyAI);
            LoadItems();
        }
    }
}

[System.Serializable]
public class Difficulty
{
    public string mode;
    public float speedAI;
    public float accuracyAI;
}

