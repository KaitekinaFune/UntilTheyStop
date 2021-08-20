using UnityEngine;

public class PlayerTransformHolder : Singleton<PlayerTransformHolder>
{
    [SerializeField] private Transform _PlayerTransform;

    public Transform PlayerTransform => _PlayerTransform;
}
