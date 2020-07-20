using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * класс для десериалзиации
 * checkers - список из пары названия спрайта шайбы и ее цены
 * modificators - список из пары названи спрайта модификатора и его цены
 */

/*[System.Serializable]
public class ShopData
{
    public List<Pair<string, int>> checkers = new List<Pair<string, int>>();
    public List<Pair<string, int>> modificators = new List<Pair<string, int>>();
}*/

/*
 * класс для сериалзиации
 * userCheckers - список шайб доступных для покупки, где True - доступна для покупки, False - шайба уже куплена
 * userModificators - спиок кол-во купленных модификатор у игрока
 * 
 */

[System.Serializable]
public class UserShopData
{
    public List<bool> userCheckers = new List<bool>();
    public List<int> userModificators = new List<int>();
}

public class Shop : MonoBehaviour
{
    public static GameObject PlayerMoneyText; //текст денег в меню
   // public GameObject checkerToBuy; //истинная шайба
   // public GameObject modificatorToBuy; //истинный модификатор
    public GameObject checkerPanel; //обьект в кот ром находятся шайбы, которые можно купить
    public GameObject modificatorsPanel; //обьект в котором находятся модификаторы, которые можно купить
    private GameObject secondTab; //2-ое меню
    public GameObject selected_panel; //выбранная панель
    GameObject currenChecker; //обьект выбранной шайбы

    PlayerData playerData;  //данные игрока
    //ShopData shopData; //данные об шайбах в магазине
    UserShopData userShopData; //данные о доступных шайбах

    const string SHOP_PATH = "Sprites/Shop/"; // путь к спрайтам магазина
    string haveIt = "Have it", selected = "Selected";

    void Start()
    {
        /*
         * Need fix
        //загрузка данных магазина
        shopData = new ShopData();
        XMLManager.LoadShop(ref shopData);
        AddCheckers(shopData.checkers.Count);
        AddModificators(shopData.modificators.Count);
        */

        //установка кол-во денег и текущий спрайт шайбы игрока
        playerData = PlayerData.getInstance();
        PlayerMoneyText = this.transform.GetChild(4).transform.GetChild(1).gameObject;
        LoadMoney();

        //установка доступных шайб для покупки
        userShopData = new UserShopData();
        LoadUserData();
    }

    //добалвение шайб (обьектов) в меню магазина
    /*public void AddCheckers(int count)
    {
        //получение координат исходной шайбы
        RectTransform rectTransform = checkerToBuy.transform.GetComponent<RectTransform>();
        Vector3 firstCheckerCoord = rectTransform.position;
        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        //count -1 т.к исходная шайба уже создана
        for (int i = 0; i < count-1; ++i)
        {
            //копирование исходной шайбы
            GameObject gm = (GameObject)Instantiate(checkerToBuy, firstCheckerCoord, Quaternion.identity);
            gm.transform.SetParent(checkerPanel.transform);
            RectTransform rectTransformClone = gm.transform.GetComponent<RectTransform>();

            //установка координат клона шайбы
            rectTransformClone.anchorMin = anchorMin;
            rectTransformClone.anchorMax = anchorMax;
            rectTransformClone.localScale = rectTransform.localScale;
            rectTransformClone.sizeDelta = rectTransform.sizeDelta;
        }

        float deltaCount = (float)count / 3f;
        if ((deltaCount % 10) != 0) deltaCount++;
        if (deltaCount >= 2)
        {
            for (int i = 0; i < (int)deltaCount-1; ++i)
            {
                RectTransform rectTransformPanel = checkerPanel.transform.GetComponent<RectTransform>();
                Vector2 offset = rectTransformPanel.offsetMax;
                offset.x += 350;
                rectTransformPanel.offsetMax = offset;
                //Debug.Log(ScreenOptimization.GetWorldCoord2D(checkerPanel).second.x);
            }
        }

        //установка текстуры и цены шайб в меню магазина
        for (int i = 0; i < checkerPanel.transform.childCount && i < shopData.checkers.Count; i++)
        {
            checkerPanel.transform.GetChild(i).transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + shopData.checkers[i].first);
            checkerPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = shopData.checkers[i].second.ToString();
        }
    }*/
    
    //загрузка денег в магазине
    public static void LoadMoney()
    {
        PlayerData playerData = PlayerData.getInstance();
        XMLManager.LoadData<PlayerData>(ref playerData, "PlayerData");
        PlayerMoneyText.GetComponent<Text>().text = playerData.money.ToString();
    }
    
    public void ChangeChecker(GameObject checker)
    {
        checker.transform.GetChild(2).transform.GetComponent<Text>().text = selected;
        currenChecker.transform.GetChild(2).transform.GetComponent<Text>().text = haveIt;

        checker.transform.GetChild(0).transform.GetComponent<Image>().sprite =  Resources.Load<Sprite>(SHOP_PATH+"selectedChecker");
        currenChecker.transform.GetChild(0).transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(SHOP_PATH+"unSelectedChecker");

        currenChecker = checker;

        //выбор шайбы как текущею
        playerData.puckSprite = checker.transform.GetChild(4).GetComponent<Image>().sprite.name;
        playerData.Save();
    }

    //покупка шайбы
    public void BuyChecker(GameObject checker)
    {
        //получение цены шайбы
        int money;
        try
        {
             money = int.Parse(checker.transform.GetChild(2).GetComponent<Text>().text);
        }
        catch
        {
            money = 0;
        }
  
        //поиск номера этой шайбы в списке
        for (int i = 0; i < checkerPanel.transform.childCount; ++i)
        {
            if (checker.transform.GetChild(4).GetComponent<Image>().sprite.name == checkerPanel.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite.name)
            {
                //если фишка свободна для покупки (True)
                if (userShopData.userCheckers[i])
                {
                    //если у юзера есть такое кол-во денег, происходит покупка
                    if (playerData.money >= money)
                    {
                        //меняем текст
                        ChangeChecker(checker);

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
                else
                {
                    if(currenChecker != checker)
                        ChangeChecker(checker);
                }
            }
        }
    }

    //загрузка данных про купленные шайбы и модификаторы
    public void LoadUserData()
    {
        if (!XMLManager.LoadData<UserShopData>(ref userShopData, "UserShopData"))
        {
            //т.к первая шайба изначально доступна
            userShopData.userCheckers.Add(false);
            for (int i = 1; i < checkerPanel.transform.childCount; ++i)
                userShopData.userCheckers.Add(true);
            for (int i = 0; i < modificatorsPanel.transform.childCount; ++i)
                userShopData.userModificators.Add(0);

            XMLManager.SaveData<UserShopData>(userShopData, "UserShopData");
        }

        //в случае если в магазин были добавленны новые шайбы, список купленных шайб обновляется
        if (userShopData.userCheckers.Count != checkerPanel.transform.childCount)
        {
            int countDelta = checkerPanel.transform.childCount - userShopData.userCheckers.Count;
            for (int i = 0; i < countDelta; ++i)
                userShopData.userCheckers.Add(true);
        }

        //в случае если в магазин были добавленны новые модификаторы, список купленных модификаторов обновляется
        if (userShopData.userModificators.Count != modificatorsPanel.transform.childCount)
        {
            int countDelta = modificatorsPanel.transform.childCount - userShopData.userModificators.Count;
            for (int i = 0; i < countDelta; ++i)
                userShopData.userModificators.Add(0);
        }

        XMLManager.SaveData<UserShopData>(userShopData, "UserShopData");

        //установка доступных шайб
        for (int i = 0; i < checkerPanel.transform.childCount; ++i)
        {
            if (userShopData.userCheckers[i]) checkerPanel.transform.GetChild(i).transform.GetChild(2).gameObject.SetActive(userShopData.userCheckers[i]);
            else checkerPanel.transform.GetChild(i).transform.GetChild(2).transform.GetComponent<Text>().text = haveIt;
        };

        //поиск номера этой шайбы в списке
        for (int i = 0; i < checkerPanel.transform.childCount; ++i)
        {
            if (playerData.puckSprite == checkerPanel.transform.GetChild(i).transform.GetChild(4).GetComponent<Image>().sprite.name)
            {
                GameObject tmp = checkerPanel.transform.GetChild(i).gameObject;
                tmp.transform.GetChild(2).GetComponent<Text>().text = selected;
                tmp.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(SHOP_PATH + "selectedChecker");
                currenChecker = tmp;
            }
        }

        //установка доступных модификаторов
        for (int i = 0; i < modificatorsPanel.transform.childCount; ++i)
            modificatorsPanel.transform.GetChild(i).transform.GetChild(5).gameObject.transform.GetComponent<Text>().text = userShopData.userModificators[i].ToString();
    }

    //добалвение модификаторов (обьектов) в меню магазина
    /*public void AddModificators(int count)
    {
        //получение координат исходного модификатора
        RectTransform rectTransform = modificatorToBuy.transform.GetComponent<RectTransform>();
        Vector3 firstModificatorrCoord = rectTransform.position;
        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        for (int i = 0; i < count - 1; ++i)
        {
            //копирование исходного модификатора
            GameObject gm = (GameObject)Instantiate(modificatorToBuy, firstModificatorrCoord, Quaternion.identity);
            gm.transform.SetParent(modificatorsPanel.transform);
            RectTransform rectTransformClone = gm.transform.GetComponent<RectTransform>();

            //перемещение клона модификатора вправо
            anchorMin.x += 0.32f;
            anchorMax.x += 0.32f;
            
            //установка координат клона модификатора
            rectTransformClone.anchorMin = anchorMin;
            rectTransformClone.anchorMax = anchorMax;
            rectTransformClone.localScale = rectTransform.localScale;
            rectTransformClone.sizeDelta = rectTransform.sizeDelta;
        }

        //установка текстуры и цены модификатора в меню магазина
        for (int i = 0; i < modificatorsPanel.transform.childCount && i < shopData.modificators.Count; i++)
        {
            modificatorsPanel.transform.GetChild(i).transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UI/Shop/" + shopData.modificators[i].first);
            modificatorsPanel.transform.GetChild(i).transform.GetChild(0).GetComponent<Text>().text = shopData.modificators[i].second.ToString();
        }
    }*/

    //покупка модификатора
    public void BuyModificator(GameObject modificator)
    {
        //получение цены модификатора
        int money = int.Parse(modificator.transform.GetChild(2).transform.GetComponent<Text>().text);

        Debug.Log(money);

        //поиск номера этого модификатора в списке
        for (int i = 0; i < modificatorsPanel.transform.childCount; ++i)
        {
            if (modificator.transform.GetChild(4).GetComponent<Image>().sprite.name == modificatorsPanel.transform.GetChild(i).GetChild(4).GetComponent<Image>().sprite.name)
            {
                Debug.Log("founded!");
                //если у юзера есть такое кол-во денег, происходит покупка
                if (playerData.money >= money)
                {
                    int count = int.Parse(modificator.transform.GetChild(5).GetComponent<Text>().text);
                    count++;
                    modificator.transform.GetChild(5).GetComponent<Text>().text = count.ToString();

                    userShopData.userModificators[i] = count;
                    XMLManager.SaveData<UserShopData>(userShopData, "UserShopData");

                    //уменьшаем деньги у игрока
                    playerData.money -= money;
                    playerData.Save();
                    LoadMoney();
                }
                else
                {
                    Debug.Log("Not enough money to buy or you already have it!");
                }
            }
        }
    }

    //firstTab - selected tab
    //secondTab - unSelected tab
    public void setTab(GameObject firstTab)
    {
        Color blue = new Color32(0, 130, 202, 255);
        Color gray = new Color32(204, 204, 204, 255);
        Color darkGray = new Color32(125, 125, 125, 255);

        firstTab.transform.GetComponent<Image>().color = blue;
        secondTab.transform.GetComponent<Image>().color = gray;

        firstTab.transform.GetChild(0).transform.GetComponent<Text>().color = Color.white;
        secondTab.transform.GetChild(0).transform.GetComponent<Text>().color = darkGray;

        firstTab.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(SHOP_PATH + "shop_selectedOption");
        secondTab.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(SHOP_PATH + "shop_unselectedOption");

        if (firstTab.tag == "modificatorBtn") selected_panel.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(SHOP_PATH + "2shop_selected_panel");
        else selected_panel.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>(SHOP_PATH + "shop_selected_panel");
    }

    public void setSecondTab(GameObject obj)
    {
        secondTab = obj;
    }
}
