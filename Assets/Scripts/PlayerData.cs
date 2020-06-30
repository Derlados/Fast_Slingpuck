using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Этот класс должен будет хранить все данные пользователя
[System.Serializable]
public class PlayerData
{
    static PlayerData instance;

    public int money;
    public List<List<bool>> progress = new List<List<bool>>();

    public static PlayerData getInstance()
    {
        if (instance == null)
        {
            instance = new PlayerData();
            instance.Init();        
        }
        return instance;
    }

    public void Init()
    {
        progress.Add(new List<bool>());
        progress[0].Add(true);
        progress[0].Add(true);
        progress[0].Add(false);
        progress[0].Add(false);
        progress.Add(new List<bool>());
        progress[1].Add(true);
        progress[1].Add(true);
        progress[1].Add(false);
        progress[1].Add(false);
        XMLManager.SaveData(this, this.ToString());
    }

    private PlayerData() { }



}
