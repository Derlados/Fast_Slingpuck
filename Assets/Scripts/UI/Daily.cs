using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * класс для сериалзиации
 * prevDay - предыдущая дата
 * showed - true-в этот день бонус был взят, false-бонус не был взят
 * curDay - номер текущего дня
 * nextDay - номер следующего дня
 */

[System.Serializable]
public class Date
{
    public DateTime prevDate;
    public bool showed;
    public int curDay;
    public int nextDay;

    public Date(DateTime prevDate, bool showed,int curDay,int nextDay)
    {
        this.prevDate = prevDate;
        this.showed = showed;
        this.curDay = curDay;
        this.nextDay = nextDay;
    }

    private Date() { }
}

// Класс отвечающий за получение ежедневного бонуса
public class Daily : MonoBehaviour
{
    public GameObject DailyMenu; //меню бонуса
    public GameObject Days; //массив дней
    public GameObject claimBtn;
    public GameObject closeBtn;

    Date date;
    UserShopData data;

    void Start()
    {
        date = new Date(System.DateTime.Today, false,0,0);
        data = new UserShopData();

        if (!XMLManager.LoadData<UserShopData>(ref data, "UserShopData"))
            Debug.LogWarning("UserShopData.xml not found. Maybe because it's the first run?");

        //отрисовка номера дней
        for (int i = 1; i <= Days.transform.childCount; ++i)
            Days.transform.GetChild(i-1).GetChild(1).GetComponent<Text>().text += " " + i.ToString();

        //загрузка сохраненого дня
        if (!XMLManager.LoadData<Date>(ref date, "daily"))
            XMLManager.SaveData<Date>(date, "daily");

        //если начался следующий день, а в предыдущем бонус был взят, то в новом дне ставится возможность забрать бонус
        if ((System.DateTime.Today > date.prevDate) && date.showed == true)
        {
            if (date.nextDay == 9)
            {
                date.curDay = 0;
                date.nextDay = 0;
            }

            date.curDay = date.nextDay;
            date.showed = false;
        }

        //отрисовка бонусов
        for (int i = 0; i < date.curDay; ++i)
            Days.transform.GetChild(i).GetChild(5).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Daily/day_claimed");
        Days.transform.GetChild(date.curDay).GetChild(5).gameObject.SetActive(false);

        //если начался следующий день и бонус не был взят, то становится активным окно взтие бонуса
        if ((System.DateTime.Today >= date.prevDate) && date.showed == false)
        {
            DailyMenu.SetActive(true);
            date.prevDate = System.DateTime.Today;
            XMLManager.SaveData<Date>(date, "daily");
        }
        else
        {
            claimBtn.SetActive(false);
            closeBtn.SetActive(true);
        }
    }

    public void Claim()
    {
        switch (Days.transform.GetChild(date.curDay).tag)
        {
            case "100": addMoney(100); break;
            case "250": addMoney(250); break;
            case "500": addMoney(500); break;
            case "1000": addMoney(1000); break;
            case "booster": addModificator(0);  break;
            case "reductor": addModificator(1);  break;
        }

        date.showed = true;
        date.nextDay++;
        XMLManager.SaveData<Date>(date, "daily");

        claimBtn.SetActive(false);
        closeBtn.SetActive(true);
        DailyMenu.SetActive(false);

        AudioManager.PlaySound(AudioManager.Audio.claim);
    }

    public void addMoney(int money)
    {
        PlayerData playerData = PlayerData.getInstance();
        playerData.money += money;
        playerData.Save();
        MainMenu.LoadMoney();
    }

    public void addModificator(int num)
    {
        data.userModificators[num] += 5;
        XMLManager.SaveData<UserShopData>(data, "UserShopData");
    }
}
