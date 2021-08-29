using Managers;
using UnityEngine;

namespace Audio
{
    public class AudioHandler
    {
        public readonly AudioType Type;

        private readonly float PitchMin;
        private readonly float PitchMax;
        private readonly float PlayCooldown;
        private readonly AudioSource Source;

        private float LastPlayTime;

        public AudioHandler(AudioData audioData, AudioSource source)
        {
            Type = audioData.Type;
            
            PitchMin = audioData.PitchMin;
            PitchMax = audioData.PitchMax;
            
            Source = source;
            Source.clip = audioData.Sound;
            Source.volume = audioData.Volume;

            PlayCooldown = audioData.Cooldown;
        }

        public void Play()
        {
            if (Time.time < LastPlayTime + PlayCooldown)
                return;

            LastPlayTime = Time.time;
            var randomPitch = Random.Range(PitchMin, PitchMax);
            Source.pitch = randomPitch;
            Source.Play();
        }
    }
}