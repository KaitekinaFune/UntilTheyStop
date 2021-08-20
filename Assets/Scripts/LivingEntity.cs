using System;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField] private float StartingHealth;
    
    private float Health;
    private bool Dead;
    
    public event Action OnDeath;
    public event Action OnHealthChange;
    
    protected virtual void Start()
    {
        Health = StartingHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        if (Dead)
            return;

        Health -= damageAmount;
        OnHealthChange?.Invoke();

        if (Health <= 0f)
        {
            Health = 0f;
            Die();
        }
    }

    protected virtual void Die()
    {
        Dead = true;
        OnDeath?.Invoke();
    }
}