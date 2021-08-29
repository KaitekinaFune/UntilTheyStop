using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(menuName = "AudioData", fileName = "New AudioData")]
    public class AudioData : ScriptableObject
    {
        public AudioClip Sound;
        public AudioType Type;
        public float Pitch;
        public float Volume;
    }
}