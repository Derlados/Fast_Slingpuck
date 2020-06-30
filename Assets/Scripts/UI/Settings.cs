using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Settings : MonoBehaviour
{
    public Text chosenModeText;
    XMLManager manager;
    private void Start()
    {
        manager = XMLManager.getInstance();
        manager.LoadItems();
        chosenModeText.text = manager.difficulty.mode;

    }

    public void BegginerPressed()
    {
        chosenModeText.text = Difficulties.Begginer.mode;
        manager.SaveItems(Difficulties.Begginer.mode, Difficulties.Begginer.speed, Difficulties.Begginer.accuracyAI, Difficulties.Begginer.timeRest);
    }

    public void SkilledPressed()
    {
        chosenModeText.text = Difficulties.Skilled.mode;
        manager.SaveItems(Difficulties.Skilled.mode, Difficulties.Skilled.speed, Difficulties.Skilled.accuracyAI, Difficulties.Skilled.timeRest);
    }

    public void MasterPressed()
    {
        chosenModeText.text = Difficulties.Master.mode;
        manager.SaveItems(Difficulties.Master.mode, Difficulties.Master.speed, Difficulties.Master.accuracyAI, Difficulties.Master.timeRest);
    }

    public void GodPressed()
    {
        chosenModeText.text = Difficulties.God.mode;
        manager.SaveItems(Difficulties.God.mode, Difficulties.God.speed, Difficulties.God.accuracyAI, Difficulties.God.timeRest);
    }

    public void ChinesePressed()
    {
        chosenModeText.text = Difficulties.Chinese.mode;
        manager.SaveItems(Difficulties.Chinese.mode, Difficulties.Chinese.speed, Difficulties.Chinese.accuracyAI, Difficulties.Chinese.timeRest);
    }

    public void RussianPressed()
    {
        LocalizationManager.curLanguage = LocalizationManager.language.RU;
        LocalizationManager.resetLanguage();
    }

    public void EnglishPressed()
    {
        LocalizationManager.curLanguage = LocalizationManager.language.EN;
        LocalizationManager.resetLanguage();
    }

    public void UkranianPressed()
    {
        LocalizationManager.curLanguage = LocalizationManager.language.UA;
        LocalizationManager.resetLanguage();
    }
}


