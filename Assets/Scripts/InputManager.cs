using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    [HideInInspector] public float HorizontalInput;
    [HideInInspector] public float VerticalInput;

    public UnityEvent OnFire1ButtonPressed;
    public UnityEvent OnFire2ButtonPressed;
    public UnityEvent OnFire3ButtonPressed;

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