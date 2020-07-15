using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * класс для десериалзиации
 * checkers - список из пары названия спрайта шайбы и ее цены
 */

[System.Serializable]
public class ShopData
{
    public List<Pair<string,int>> checkers = new List<Pair<string, int>>();
}

/*
 * класс для сериалзиации
 * userCheckers - список шайб доступных для покупки, где True - доступна для покупки, False - шайба уже куплена
 */

[System.Serializable]
public class UserShopData
{
    public List<bool> userCheckers = new List<bool>();
}

public class Shop : MonoBehaviour
{
    public static GameObject PlayerMoneyText;
    public GameObject currentChecker;
    PlayerData playerData; 
    ShopData shopData; //данные об шайбах в магазине
    public GameObject checkerToBuy;
    public GameObject CheckerPanel; //обьект в котором находятся шайбы, которые можно купить
    UserShopData userShopData;

    void Start()
    {
        //загрузка данных магазина
        shopData = new ShopData();
        XMLManager.LoadShop(ref shopData);
        AddCheckers(shopData.checkers.Count);

        //установка кол-во денег и текущий спрайт шайбы игрока
        playerData = PlayerData.getInstance();
        currentChecker.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + playerData.puckSprite);
        PlayerMoneyText = this.transform.GetChild(2).transform.GetChild(2).gameObject;
        LoadMoney();

        //установка доступных шайб для покупки
        userShopData = new UserShopData();
        LoadCheckers();
    }

    //добалвение шайб (обьектов) в меню магазина
    public void AddCheckers(int count)
    {
        //получение координат исходной шайбы
        RectTransform rectTransform = checkerToBuy.transform.GetComponent<RectTransform>();
        Vector3 firstCheckerCoord = rectTransform.position;
        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        for(int i = 0; i < count-1; ++i)
        {
            //копирование исходной шайбы
            GameObject gm = (GameObject)Instantiate(checkerToBuy, firstCheckerCoord, Quaternion.identity);
            gm.transform.SetParent(CheckerPanel.transform);
            RectTransform rectTransformClone = gm.transform.GetComponent<RectTransform>();

            if (anchorMin.x > 0.48f)
            {
                //если дошли до границы меню, смещаем клон шайбы на 2 строку
                anchorMin.x = rectTransform.anchorMin.x;
                anchorMax.x = rectTransform.anchorMax.x;
                anchorMin.y -= 0.32f;
                anchorMax.y -= 0.32f;
            }
            else
            {
                //перемещение клона шайбы вправо
                anchorMin.x += 0.32f;
                anchorMax.x += 0.32f;
            }

            //установка координат клона шайбы
            rectTransformClone.anchorMin = anchorMin;
            rectTransformClone.anchorMax = anchorMax;
            rectTransformClone.localScale = rectTransform.localScale;
            rectTransformClone.sizeDelta = rectTransform.sizeDelta;
        }

        //установка текстуры и цены шайб в меню магазина
        for (int i = 0; i < CheckerPanel.transform.childCount && i < shopData.checkers.Count; i++)
        {
            CheckerPanel.transform.GetChild(i).transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + shopData.checkers[i].first);
            CheckerPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = shopData.checkers[i].second.ToString();
        }
    }
    
    //загрузка денег в магазине
    public static void LoadMoney()
    {
        PlayerData playerData = PlayerData.getInstance();
        XMLManager.LoadData<PlayerData>(ref playerData, "PlayerData");
        PlayerMoneyText.GetComponent<Text>().text = playerData.money.ToString();
    }

    //покупка шайбы
    public void BuyChecker(GameObject checker)
    {
        //получение цены шайбы
        int money = int.Parse(checker.transform.GetChild(0).transform.GetComponent<Text>().text);

        //выбор шайбы как текущею
        Sprite puckSprite = checker.GetComponent<Image>().sprite;
        currentChecker.GetComponent<Image>().sprite = puckSprite;
        playerData.puckSprite = puckSprite.name;
        playerData.Save();

        //поиск номера этой шайбы в списке
        for (int i = 0; i < shopData.checkers.Count; ++i)
        {
            if (checker.GetComponent<Image>().sprite.name == CheckerPanel.transform.GetChild(i).GetComponent<Image>().sprite.name)
            {
                //если фишка свободна для покупки (True) и если у юзера есть такое кол-во денег, происходит покупка
                if (userShopData.userCheckers[i] && playerData.money >= money)
                {
                    //выключаем текст цены
                    checker.transform.GetChild(0).gameObject.SetActive(false);

                    //записываем эту фишку как купленную
                    userShopData.userCheckers[i] = false;
                    XMLManager.SaveData<UserShopData>(userShopData, "UserShopData");

                    //уменьшаем деньги у игрока
                    playerData.money -= money;
                    playerData.Save();
                    LoadMoney();
                }
                else
                {
                    Debug.Log("Not enough money to buy!");
                }
            }
        }

    }

    //загрузка купленных шайб
    public void LoadCheckers()
    {
        if (!XMLManager.LoadData<UserShopData>(ref userShopData, "UserShopData"))
        {
            for (int i = 0; i < shopData.checkers.Count; ++i)
                userShopData.userCheckers.Add(true);
            XMLManager.SaveData<UserShopData>(userShopData, "UserShopData");
        }

        //в случае если в магазин были добавленны новые шайбы, список купленных шайб обновляется
        if(userShopData.userCheckers.Count != shopData.checkers.Count)
        {
            int countDelta = shopData.checkers.Count - userShopData.userCheckers.Count;
            for (int i = 0; i < countDelta; ++i)
                userShopData.userCheckers.Add(true);
            
            XMLManager.SaveData<UserShopData>(userShopData, "UserShopData");
        }
    
        //установка доступных шайб
        for (int i = 0; i < shopData.checkers.Count; ++i)
            CheckerPanel.transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(userShopData.userCheckers[i]);
    }
}
