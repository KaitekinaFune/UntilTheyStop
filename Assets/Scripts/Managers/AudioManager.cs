using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils;

namespace Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioSource AudioSource;
        [SerializeField] private List<AudioData> AudioDatas;

        public void Play(AudioType type)
        {
            foreach (var audioData in AudioDatas.Where(audioData => audioData.Type == type))
            {
                AudioSource.pitch = audioData.Pitch;
                AudioSource.volume = audioData.Volume;
                AudioSource.clip = audioData.Sound;
            }

            AudioSource.Play();
        }
    }
}