using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager 
{
    public enum Audio
    {
        click,
        checker_hit,
        zoom,
        unzoom,
        string_pulling,
        buy,
        refuse,
        select,
        rise01,
        rise02,
        rise03,
        rise04,
        claim,
        modificator,
        count,
        endCount
    }

    public static void PlaySound(Audio audio)
    {
        GameObject audioGameObject = new GameObject("Audio");
        AudioSource audioSource = audioGameObject.AddComponent<AudioSource>();
        audioSource.volume = PlayerData.getInstance().effectsVolume;
        AudioClip audioClip = GetAudioClip(audio);
        audioSource.PlayOneShot(audioClip);
        UnityEngine.Object.Destroy(audioGameObject, audioClip.length);
    }


    private static AudioClip GetAudioClip(Audio audio)
    {
        foreach(AudioAssets.SoundAudioClip soundAudioClip in AudioAssets.i.SoundAudioClipArray)
        {
            if (soundAudioClip.audio == audio)
                return soundAudioClip.audioClip;
        }
        Debug.LogError("Sound " + audio + " not found!");
        return null;
    }
}
