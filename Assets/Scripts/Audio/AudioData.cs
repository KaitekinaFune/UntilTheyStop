using UnityEngine;
using AudioType = Audio.AudioType;

namespace Managers
{
    [CreateAssetMenu(menuName = "AudioData", fileName = "New AudioData")]
    public class AudioData : ScriptableObject
    {
        public AudioClip Sound;
        public AudioType Type;
        public float PitchMin;
        public float PitchMax;
        public float Volume;
        public float Cooldown;
    }
}