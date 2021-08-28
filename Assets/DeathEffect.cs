using System;
using UnityEngine;

public class DeathEffect : MonoBehaviour, IResettable
{
    public event Action OnAnimationEnd;

    public void EndAnimation()
    {
        OnAnimationEnd?.Invoke();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }
}
