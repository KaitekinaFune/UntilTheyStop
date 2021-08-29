using System;
using Pools;
using UnityEngine;

namespace Projectiles
{
    public class DamageZone : MonoBehaviour, IResettable
    {
        public event Action OnAnimationEnd;
        public event Action<DamageZone> OnDamageActivated;
    
        public void Reset()
        {
            gameObject.SetActive(false);
        }

        public void Spawn(Vector3 point)
        {
            transform.position = point;
            gameObject.SetActive(true);
        }

        private void InvokeOnAnimationEnd()
        {
            OnAnimationEnd?.Invoke();
        }
    
        private void InvokeOnDamageActivated()
        {
            OnDamageActivated?.Invoke(this);
        }
    }
}