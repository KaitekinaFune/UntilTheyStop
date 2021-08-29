using UnityEngine;
using AudioType = Managers.AudioType;

[CreateAssetMenu(menuName = "AudioData", fileName = "New AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip Sound;
    public AudioType Type;
}