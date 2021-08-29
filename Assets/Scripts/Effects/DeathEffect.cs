using System;
using Pools;
using UnityEngine;

namespace Effects
{
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
}
