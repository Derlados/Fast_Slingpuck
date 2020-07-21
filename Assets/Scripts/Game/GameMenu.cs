using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml.Linq;

// Класс отвечающий за весь UI в самой игре
public class GameMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas, gameOverCanvas, capperField, PauseBtnCanvas;
    public Text gameOverText, scoreText;
    PlayerData playerData;

    public void Start()
    {
        playerData = PlayerData.getInstance();
    }
    public void Pause()
    {
        Time.timeScale = 0f;
        capperField.SetActive(true);
        pauseMenuCanvas.SetActive(true);
        PauseBtnCanvas.SetActive(false);
    }
    public void UnPause()
    {
        Time.timeScale = 1f;
        capperField.SetActive(false);
        pauseMenuCanvas.SetActive(false);
        PauseBtnCanvas.SetActive(true);
    }
    public void ToMainMenuPressed()
    {
        Debug.Log("GGG");
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameMenu");
    }
    public void gameOver(string message, int stars, int money1, int money2, int money3)
    {
        PlayerData playerData = PlayerData.getInstance();

        createGameOverMenu();
        gameOverCanvas.SetActive(true);
        capperField.SetActive(true);
        StartCoroutine(spawnStars(stars, money1, money2, money3));

        // Запись значений в PlayerData
        playerData.money += money1 + money2 + money3;
        if (playerData.progress[GameRule.planetNum].first[GameRule.levelNum] < stars)
            playerData.progress[GameRule.planetNum].first[GameRule.levelNum] = (byte)stars;

        if (GameRule.levelNum == GameRule.levelsCount && message== "YOU WIN !")
        {
           playerData.currentPlanet++;
           playerData.progress[playerData.currentPlanet].second.second = true;
        }

        XMLManager.SaveData(PlayerData.getInstance(), PlayerData.getInstance().ToString());

        gameOverText.text = message;

    }

    [System.Serializable]
    public class GameOverMenu
    {
        public List<Image> stars; // звезды
        public Image moneyTargetImage1, moneyTargetImage2, moneyTargetImage3; // Картинки критериев для получения монет
        public Text targetText1, targetText2, targetText3, totalText; // текст критериев получения монет
        public Text moneyTargetText1, moneyTargetText2, moneyTargetText3, moneyTotalText; // текст количество монет
        public Text gameOver; // Текст конца игры
    }
    public GameOverMenu gameOverMenu;

    void createGameOverMenu()
    {
        XElement data; // Данные XML файла
        TextAsset textAsset = (TextAsset)Resources.Load("XML/Lozalization/" + LocalizationManager.curLanguage.ToString() + "/GameOverMenu");
        data = XDocument.Parse(textAsset.text).Element("GameOverMenu");

        gameOverMenu.gameOver.text = data.Element("gameOver").Value;
        gameOverMenu.totalText.text = data.Element("totalText").Value;

        data = data.Element(GameRule.mode.ToString());

        XElement images = data.Element("images"); // названия спрайтов
        gameOverMenu.moneyTargetImage1.sprite = Resources.Load<Sprite>("Sprites/GameOverMenu/" + images.Element("targetImage1").Value);
        gameOverMenu.moneyTargetImage2.sprite = Resources.Load<Sprite>("Sprites/GameOverMenu/" + images.Element("targetImage2").Value);
        gameOverMenu.moneyTargetImage3.sprite = Resources.Load<Sprite>("Sprites/GameOverMenu/" + images.Element("targetImage3").Value);

        XElement texts = data.Element("texts");
        gameOverMenu.targetText1.text = texts.Element("targetText1").Value;
        gameOverMenu.targetText2.text = texts.Element("targetText2").Value;
        gameOverMenu.targetText3.text = texts.Element("targetText3").Value;
    }

    IEnumerator spawnStars(int stars, int money1, int money2, int money3)
    {
        for (int i = 0; i < stars; ++i)
        {
            gameOverMenu.stars[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(spawnCount(money1, money2, money3));
    }
    IEnumerator spawnCount(int money1, int money2, int money3)
    {
        for (int i = 1; i <= money1; ++i)
        {
            gameOverMenu.moneyTargetText1.text = i.ToString();
            yield return new WaitForSeconds(0.015f);
        }

        for (int i = 1; i <= money2; ++i)
        {
            gameOverMenu.moneyTargetText2.text = i.ToString();
            yield return new WaitForSeconds(0.015f);
        }

        for (int i = 1; i <= money3; ++i)
        {
            gameOverMenu.moneyTargetText3.text = i.ToString();
            yield return new WaitForSeconds(0.015f);
        }

        int total = money1 + money2 + money3;
        for (int i = 1; i <= total; ++i)
        {
            gameOverMenu.moneyTotalText.text = i.ToString();
            yield return new WaitForSeconds(0.015f);
        }
    }
}

