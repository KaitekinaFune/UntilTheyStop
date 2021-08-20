using System;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    [HideInInspector] public float HorizontalInput;
    [HideInInspector] public float VerticalInput;

    public event Action OnFire1ButtonPressed;
    public event Action OnFire2ButtonPressed;
    public event Action OnFire3ButtonPressed;

    private void Update()
    {
        HorizontalInput = Input.GetAxisRaw("Horizontal");
        VerticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetAxis("Fire1") > 0f)
            OnFire1ButtonPressed?.Invoke();
        if (Input.GetAxis("Fire2") > 0f)
            OnFire2ButtonPressed?.Invoke();
        if (Input.GetAxis("Fire3") > 0f)
            OnFire3ButtonPressed?.Invoke();
    }
}