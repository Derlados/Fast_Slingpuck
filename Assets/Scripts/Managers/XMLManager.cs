using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System;
using System.Xml.Linq;
using System.Globalization;
using BaseStructures;

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
        
        FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + name + ".xml", FileMode.Create); //ANDROID
        //FileStream fileStream = new FileStream(Application.dataPath + "/Resources/" + name + ".xml", FileMode.Create); //PC
        serializer.Serialize(fileStream, data);
        fileStream.Close();
    }

    /* Универсальная десериализация
     * Параметры:
     * data - класс который необходимо сериализовать 
     * name - имя xml файла 
     * Возврат:
     * true - десериализация успешна
     * false - отсутствует файл
     */
    public static bool LoadData<T>(ref T data, string name)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        try
        {
            FileStream fileStream = new FileStream(Application.persistentDataPath + "/" + name + ".xml", FileMode.Open); //ANDROID
            //FileStream fileStream = new FileStream(Application.dataPath + "/Resources/" + name + ".xml", FileMode.Open); //PC
            data = (T)serializer.Deserialize(fileStream);
            fileStream.Close();
            return true;
        }
        catch
        {
            return false;
        }
    }

    /* Универсальная загрузка сложностей
     * Параметры:
     * data - класс который необходимо сериализовать
     * key - уровень сложности
     */
    public static void LoadDifficulty(ref Difficulty data, string key)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Game/Difficulties");
        XElement xdoc = XDocument.Parse(textAsset.text).Element("difficulties");

        foreach (XElement diff in xdoc.Elements(key))
        {
            data.mode = diff.Element("mode").Value;
            data.speedAI = float.Parse(diff.Element("speedAI").Value);
            data.accuracyAI = float.Parse(diff.Element("accuracyAI").Value, CultureInfo.InvariantCulture);
            data.timeRest = float.Parse(diff.Element("timeRest").Value, CultureInfo.InvariantCulture);
        }
    }

    /* Универсальная загрузка сложностей
     * Параметры:
     * data - класс который необходимо сериализовать
     * planet - название планеты (берется из GameRule.Type)
     * level - номер уровня на планете
     */
    public static void LoadLevel(ref LevelData data, string planet, int level)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Game/levels");
        XElement xdoc = XDocument.Parse(textAsset.text).Element("levels");

        foreach (XElement pln in xdoc.Elements(planet))
        {
            foreach (XElement lvl in pln.Elements("lvl_" + level.ToString()))
            {
                foreach (XElement normal in lvl.Elements("normal"))
                {
                    data.time = int.Parse(normal.Element("time").Value);
                    data.countCheckers = int.Parse(normal.Element("countCheckers").Value);
                }

                foreach (XElement speed in lvl.Elements("speed"))
                {
                    data.minTargetCheckers = int.Parse(speed.Element("minTargetCheckers").Value);
                    data.maxTargetCheckers = int.Parse(speed.Element("maxTargetCheckers").Value);
                    data.accuracy = int.Parse(speed.Element("accuracy").Value);
                }

                data.mode = (GameRule.Mode)System.Enum.Parse(typeof(GameRule.Mode), lvl.Element("mode").Value);
                data.typeAI = (GameRule.AI)System.Enum.Parse(typeof(GameRule.AI), lvl.Element("typeAI").Value);
                data.typeGate = (GameRule.Gate)System.Enum.Parse(typeof(GameRule.Gate), lvl.Element("typeGate").Value);
                data.difficulties = (GameRule.Difficulties)System.Enum.Parse(typeof(GameRule.Difficulties), lvl.Element("difficulties").Value);

                foreach (XElement chkMod in lvl.Elements("AIModifier"))
                {
                    int size = int.Parse(chkMod.Element("size").Value);
                    for (int i = 0; i < size; ++i)
                        data.AIModifier.Add((GameRule.CheckerModifier)System.Enum.Parse(typeof(GameRule.CheckerModifier), chkMod.Element("checkerModifier" + i).Value));
                }
            }
        }
    }
    

    /* Универсальная загрузка данных в магазин
     * Параметры:
     * data - класс который необходимо сериализовать
     */
    /*public static void LoadShop(ref ShopData data)
    {
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Shop/ShopData");
        XElement shopData = XDocument.Parse(textAsset.text).Element("ShopData");

        foreach (XElement checkers in shopData.Elements("checkers"))
        {
            foreach (XElement diff in checkers.Elements("PairOfStringInt32"))
            {
                Pair<string, int> pair = new Pair<string, int>();
                pair.first = diff.Element("first").Value;
                pair.second = int.Parse(diff.Element("second").Value);
                data.checkers.Add(pair);
            }
        }

        foreach (XElement checkers in shopData.Elements("modificators"))
        {
            foreach (XElement diff in checkers.Elements("PairOfStringInt32"))
            {
                Pair<string, int> pair = new Pair<string, int>();
                pair.first = diff.Element("first").Value;
                pair.second = int.Parse(diff.Element("second").Value);
                data.modificators.Add(pair);
            }
        }

    }*/
}
