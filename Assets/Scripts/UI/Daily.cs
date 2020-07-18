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

    void Start()
    {
        date = new Date(System.DateTime.Today, false,1,1);

        //загрузка сохраненого дня
        if (!XMLManager.LoadData<Date>(ref date, "daily"))
            XMLManager.SaveData<Date>(date, "daily");

        //если начался следующий день, а в предыдущем бонус был взят, то в новом дне ставится возможность забрать бонус
        if ((System.DateTime.Today > date.prevDate) && date.showed == true)
        {
            date.curDay = date.nextDay;
            date.showed = false;
        }

        //отрисовка бонусов
        for (int i = 0; i < date.curDay; ++i)
            Days.transform.GetChild(i).GetChild(5).GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Daily/day_claimed");
        Days.transform.GetChild(date.curDay).GetChild(5).gameObject.SetActive(false);

        //date.showed = false;
        //если начался следующий день и бонус не был взят, то становится активным окно взтие бонуса
        if ((System.DateTime.Today >= date.prevDate) && date.showed == false)
        {
            DailyMenu.SetActive(true);

            if (date.nextDay == 9)
            {
                date.curDay = 1;
                date.nextDay = 1;
            }

            date.prevDate = System.DateTime.Today;
            date.nextDay++;

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
        PlayerData playerData = PlayerData.getInstance();
        playerData.money += 100;
        playerData.Save();
        MainMenu.LoadMoney();

        date.showed = true;
        claimBtn.SetActive(false);
        closeBtn.SetActive(true);
        DailyMenu.SetActive(false);
        XMLManager.SaveData<Date>(date, "daily");
    }
}
