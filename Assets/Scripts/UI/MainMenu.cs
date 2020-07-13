using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public static GameObject PlayerMoneyText;

    void Start()
    {
        PlayerMoneyText = this.transform.GetChild(0).transform.GetChild(2).gameObject;
        LoadMoney();
    }

    public static void LoadMoney()
    {
        PlayerData playerData = PlayerData.getInstance();
        XMLManager.LoadData<PlayerData>(ref playerData, "PlayerData");
        PlayerMoneyText.GetComponent<Text>().text = playerData.money.ToString();
    }

    public void SpeedGamePressed()
    {
        GameManager.setSpeedGame();
        SceneManager.LoadScene("Game");
    }

    public void PlayPressed()
    {
        GameManager.setNormalMode();
        SceneManager.LoadScene("Game");
    }

    public void ExitPressed()
    {
        Application.Quit();
    }


}
