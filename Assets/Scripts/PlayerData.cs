using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using System.IO;
using BaseStructures;
using UnityEditor;
using System;

// Этот класс должен будет хранить все данные пользователя
[System.Serializable]
public class PlayerData
{
    private static PlayerData instance;

    public int money;
    public List<Pair<List<byte>, Pair<GameRule.Type, bool>>> progress = new List<Pair<List<byte>, Pair<GameRule.Type, bool>>>(); // список в котором находятся пары, сотоящие из списка уровней и доступности планет
    public string puckSprite; //выбранный спрайт игрока
    public int currentPlanet;
    public SystemLanguage lang;
    public bool checkerRotation;
    public float volume, effectsVolume;
    public int incScore = 0;
    public int loses = 0;
    
    public static PlayerData getInstance()
    {
        if (instance == null)
        {
            if (!XMLManager.LoadData(ref instance, (new PlayerData()).ToString()))
            {
                instance = new PlayerData();
                instance.puckSprite = "userChecker1"; ;
                if (Application.systemLanguage == SystemLanguage.English ||
                    Application.systemLanguage == SystemLanguage.Russian ||
                    Application.systemLanguage == SystemLanguage.Ukrainian)
                    instance.lang = Application.systemLanguage;
                else
                    instance.lang = SystemLanguage.English;

                instance.volume = 1f;
                instance.effectsVolume = 1f;
                instance.checkerRotation = false;

                instance.add();
                //ставим 0-ую планета как начальную
                instance.progress[0].second.second = true;


                instance.Save();
            }

            if(MenuManager.allPlanets != instance.progress.Count)
            {
                instance.add();
                instance.Save();
            }
        }

        return instance;
    }

    public void add()
    {
        //allPlanets - обьект galaxy
        for (int i = progress.Count; i < MenuManager.allPlanets; ++i)
        {
            progress.Add(new Pair<List<byte>, Pair<GameRule.Type, bool>>());
            progress[i].first = new List<byte>();

            //planets - обьект mainMenu
            for (int j = 0; j < MenuManager.planets.transform.GetChild(i + 5).transform.childCount; ++j)
                progress[i].first.Add(0);

            progress[i].second = new Pair<GameRule.Type, bool>(GameRule.planetProgressNum[i], false);
        }
    }

    public void Save()
    {
        XMLManager.SaveData(this, this.ToString());
    }

    private PlayerData() { }


}
