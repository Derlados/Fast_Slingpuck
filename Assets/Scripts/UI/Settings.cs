using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Difficulty
{
    public string mode;
    public float speedAI;
    public float accuracyAI;
    public float timeRest;
}

public class Settings : MonoBehaviour
{
    public Text chosenModeText;
    public Difficulty diff;
    string settingsFileName = "settings";

    private void Start()
    {
        diff = new Difficulty();

        //В случае если режим не был выбран,то по умолчанию ставится режим begginer
        try
        {
            XMLManager.LoadData<Difficulty>(ref diff, settingsFileName);
            chosenModeText.text = diff.mode;
        }
        catch
        {
            XMLManager.LoadDifficulty(ref diff, "begginer");
            XMLManager.SaveData<Difficulty>(diff, settingsFileName);
            chosenModeText.text = diff.mode;
        }
    }

    /* Универсальный выбор сложностей
     * Параметры:
     * mode - выбранный режим 
     */
    public void LoadDifficultyPressed(string mode)
    {
        XMLManager.LoadDifficulty(ref diff, mode);
        XMLManager.SaveData<Difficulty>(diff, settingsFileName);
        chosenModeText.text = diff.mode;
    }

    /* Универсальный выбор языка
     * Параметры:
     * lang - выбранный язык 
     */
    public void LanguagePressed(string lang)
    {
        switch (lang)
        {
            case "RU":
                LocalizationManager.curLanguage = LocalizationManager.language.RU;
                break;
            case "EN":
                LocalizationManager.curLanguage = LocalizationManager.language.EN;
                break;
            case "UA":
                LocalizationManager.curLanguage = LocalizationManager.language.UA;
                break;
        }
        LocalizationManager.resetLanguage();
    }
}


