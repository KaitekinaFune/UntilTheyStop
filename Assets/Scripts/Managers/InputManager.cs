using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        [HideInInspector] public float HorizontalInput;
        [HideInInspector] public float VerticalInput;

        [SerializeField] private List<KeyCode> Fire1Buttons;
        [SerializeField] private List<KeyCode> Fire2Buttons;
        [SerializeField] private List<KeyCode> Fire3Buttons;

        public UnityEvent OnFire1ButtonPressed;
        public UnityEvent OnFire2ButtonPressed;
        public UnityEvent OnFire3ButtonPressed;

        public event Action AnyKeyPressed;

        private void Update()
        {
            HorizontalInput = Input.GetAxisRaw("Horizontal");
            VerticalInput = Input.GetAxisRaw("Vertical");

            foreach (var _ in Fire1Buttons.Where(Input.GetKeyDown))
                OnFire1ButtonPressed?.Invoke();
            
            foreach (var _ in Fire2Buttons.Where(Input.GetKeyDown))
                OnFire2ButtonPressed?.Invoke();
            
            foreach (var _ in Fire3Buttons.Where(Input.GetKeyDown))
                OnFire3ButtonPressed?.Invoke();

            if (Input.anyKeyDown) 
                AnyKeyPressed?.Invoke();
        }
    }
}