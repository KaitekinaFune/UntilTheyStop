using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;
using AudioType = Managers.AudioType;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private List<AudioData> AudioDatas;

    public void Play(AudioType type, float pitch = 1f)
    {
        AudioSource.pitch = pitch;
        foreach (var audioData in AudioDatas.Where(audioData => audioData.Type == type))
            AudioSource.clip = audioData.Sound;

        AudioSource.Play();
    }
}