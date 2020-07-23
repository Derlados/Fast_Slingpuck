using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Settings : MonoBehaviour
{
    /* Универсальный выбор языка
     * Параметры:
     * lang - выбранный язык 
     */
    public void LanguagePressed(string lang)
    {
        AudioManager.PlaySound(AudioManager.Audio.select);
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


