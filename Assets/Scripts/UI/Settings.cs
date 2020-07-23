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

        PlayerData playerData = PlayerData.getInstance();
        switch (lang)
        {
            case "RU":
                playerData.lang = SystemLanguage.Russian;
                break;
            case "EN":
                playerData.lang = SystemLanguage.English;
                break;
            case "UA":
                playerData.lang = SystemLanguage.Ukrainian;
                break;
        }
        playerData.Save();
        LocalizationManager.resetLanguage();
    }
}


