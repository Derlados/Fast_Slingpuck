using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;

// Этот класс должен будет хранить все данные пользователя
[System.Serializable]
public class PlayerData
{
    private static PlayerData instance;

    public int money;
    public List<List<bool>> progress = new List<List<bool>>();

    public static PlayerData getInstance()
    {
        if (instance == null)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(PlayerData));
            try
            {
                FileStream fileStream = new FileStream(@"D:\GAMES\Unity\Projects\FastSlingpuck\Assets\Resources\PlayerData.xml", FileMode.Open); // Дебаг, для комп
                //FileStream fileStream = new FileStream(Application.persistentDataPath + "/PlayerData.xml", FileMode.Open);     
                instance = (PlayerData)(serializer.Deserialize(fileStream));
                fileStream.Close();
            }
            catch
            {
                instance = new PlayerData();
                instance.Init();
            }
        }

        return instance;
    }

    public void Init()
    {
        Debug.Log("INIT");
        XMLManager.SaveData(this, this.ToString());
    }

    private PlayerData() { }


}
