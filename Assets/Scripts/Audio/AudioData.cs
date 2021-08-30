using UnityEngine;

namespace Audio
{
    [CreateAssetMenu(menuName = "Audio Data", fileName = "New Audio Data")]
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