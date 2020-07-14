using BaseStructures;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ShopData
{
    public List<Pair<string,bool>> checkers = new List<Pair<string, bool>>();
}

public class Shop : MonoBehaviour
{
    public static GameObject PlayerMoneyText;
    public GameObject currentChecker;
    PlayerData playerData; 
    ShopData shopData; //данные об шайбах в магазине
    public GameObject checkerToBuy;
    public GameObject CheckerPanel; //обьект в котором находятся шайбы, которые можно купить

    void Start()
    {
        shopData = new ShopData();
        XMLManager.LoadShop(ref shopData);
        AddCheckers(shopData.checkers.Count);

        playerData = PlayerData.getInstance();
        currentChecker.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + playerData.puckSprite);
        PlayerMoneyText = this.transform.GetChild(2).transform.GetChild(2).gameObject;
        LoadMoney();
    }

    public void AddCheckers(int count)
    {
        RectTransform rectTransform = checkerToBuy.transform.GetComponent<RectTransform>();
        Vector3 firstCheckerCoord = rectTransform.position;
        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        for(int i = 0; i < count-1; ++i)
        {
            GameObject gm = (GameObject)Instantiate(checkerToBuy, firstCheckerCoord, Quaternion.identity);
            gm.transform.SetParent(CheckerPanel.transform);
            RectTransform rectTransformClone = gm.transform.GetComponent<RectTransform>();

            if (anchorMin.x > 0.48f)
            {;
                anchorMin.x = rectTransform.anchorMin.x;
                anchorMax.x = rectTransform.anchorMax.x;
                anchorMin.y -= 0.32f;
                anchorMax.y -= 0.32f;

            }
            else
            {
                anchorMin.x += 0.32f;
                anchorMax.x += 0.32f;
            }

            rectTransformClone.anchorMin = anchorMin;
            rectTransformClone.anchorMax = anchorMax;
            rectTransformClone.localScale = rectTransform.localScale;
            rectTransformClone.sizeDelta = rectTransform.sizeDelta;
        }

        for (int i = 0; i < CheckerPanel.transform.childCount && i < shopData.checkers.Count; i++)
        {
            CheckerPanel.transform.GetChild(i).transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/levels/checkers/" + shopData.checkers[i].first);
        }
    }
    
    public static void LoadMoney()
    {
        PlayerData playerData = PlayerData.getInstance();
        XMLManager.LoadData<PlayerData>(ref playerData, "PlayerData");
        PlayerMoneyText.GetComponent<Text>().text = playerData.money.ToString();
    }

    public void BuyChecker(GameObject checker)
    {
        int money = int.Parse(checker.transform.GetChild(0).transform.GetComponent<Text>().text);
        if (playerData.money >= money)
        {
            Sprite puckSprite = checker.GetComponent<Image>().sprite;
            currentChecker.GetComponent<Image>().sprite = puckSprite;
            checker.transform.GetChild(0).gameObject.SetActive(false);

            playerData.puckSprite = puckSprite.name;
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
