using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    public Slider gameRuleSlider; // Слайдер игровых правил
    public Slider volumeSlider, effectsVolumeSlider; // Слайдер громкости
    public AudioSource audio;

    // Устанавливаем слайдеры относительно настроек которые были записаны
    private void Start()
    {
        gameRuleSlider.value = PlayerData.getInstance().checkerRotation == false ? 0 : 1;
        volumeSlider.value = PlayerData.getInstance().volume;
        effectsVolumeSlider.value = PlayerData.getInstance().effectsVolume;
        audio.volume = volumeSlider.value;
    }

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

        Bonuses.changed = false;
    }

    // Настройка громкости
    public void setVolume()
    {
        PlayerData.getInstance().volume = volumeSlider.value;
        audio.volume = volumeSlider.value;
    }

    public void setEffectsVolume()
    {
        PlayerData.getInstance().effectsVolume = effectsVolumeSlider.value;
    }

    // Установка игровых правил
    public void serGameRule()
    {
        PlayerData.getInstance().checkerRotation = gameRuleSlider.value == 0 ? false : true;
    }
}



