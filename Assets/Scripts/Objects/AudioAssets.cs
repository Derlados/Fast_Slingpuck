using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAssets : MonoBehaviour
{
    private static AudioAssets _i;

   public static AudioAssets i
    {
        get
        {
            if (_i == null) _i = Instantiate(Resources.Load<AudioAssets>("Audio/AudioAssets"));
            return _i;
        }
    }

    private AudioAssets() { }

    public SoundAudioClip[] SoundAudioClipArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioManager.Audio audio;
        public AudioClip audioClip;
    }
}
