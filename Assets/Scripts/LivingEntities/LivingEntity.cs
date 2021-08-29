using System;
using Pools;
using UnityEngine;
using Utils;

namespace LivingEntities
{
    public abstract class LivingEntity : MonoBehaviour, IDamageable, IResettable
    {
        [SerializeField] private float StartingHealth;
    
        public event Action<LivingEntity> OnSpawn;
        public event Action<LivingEntity> OnDeath;
        public EventHandler<HealthChangeArgs> OnHealthChange;

        protected bool Dead;
    
        public float Health { get; private set; }
        public bool NeedHealing => Health < StartingHealth;

        protected virtual void Awake()
        {
            ResetHealth();
        }

        public void Spawn(Vector3 spawnPosition)
        {
            ResetHealth();
            OnSpawn?.Invoke(this);
            Dead = false;
            gameObject.transform.position = spawnPosition;
            InvokeOnHealthChange();
            Enable();
        }

        public void TakeDamage(float damageAmount)
        {
            if (Dead)
                return;

            Health -= damageAmount;
            Health = Mathf.Clamp(Health, 0f, float.MaxValue);
            InvokeOnHealthChange();

            if (Health <= 0f)
            {
                Die();
            }
        }

        public virtual void Heal(float healAmount)
        {
            if (Dead)
                return;
        
            Health += healAmount;
            Health = Mathf.Clamp(Health, 0f, StartingHealth);
            InvokeOnHealthChange();
        }

        private void InvokeOnHealthChange()
        {
            OnHealthChange?.Invoke(this, new HealthChangeArgs(StartingHealth, Health));
        }

        protected virtual void Die()
        {
            Dead = true;
            OnDeath?.Invoke(this);
        }

        private void ResetHealth()
        {
            Health = StartingHealth;
        }

        public void Reset()
        {
            gameObject.SetActive(false);
        }

        public float GetHealthPercent()
        {
            return Health / StartingHealth;
        }

        private void Enable()
        {
            gameObject.SetActive(true);
        }
    }
}