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
    public List<List<bool>> progress = new List<List<bool>>(); // Массив bool отмечающий какие уровни уже пройдены

    public static PlayerData getInstance()
    {
        if (instance == null)
            if (!XMLManager.LoadData(ref instance, (new PlayerData()).ToString()))
            {
                instance = new PlayerData();
                instance.Init();
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
