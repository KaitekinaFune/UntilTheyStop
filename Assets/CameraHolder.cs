using UnityEngine;

public class CameraHolder : Singleton<CameraHolder>
{
    public Camera Camera { get; private set; }

    private void Start()
    {
        Camera = GetComponent<Camera>();
    }
}
