using System;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable, IResettable
{
    [SerializeField] private float StartingHealth;
    
    public event Action<LivingEntity> OnSpawn;
    public event Action<LivingEntity> OnDeath;
    public EventHandler<HealthChangeArgs> OnHealthChange;

    private bool Dead;
    
    public float Health { get; private set; }
    public bool NeedHealing => Health < StartingHealth;

    protected virtual void Awake()
    {
        ResetHealth();
    }

    public void Spawn(Vector3 spawnPosition)
    {
        OnHealthChange?.Invoke(this, new HealthChangeArgs(StartingHealth, StartingHealth));
        OnSpawn?.Invoke(this);
        ResetHealth();
        Dead = false;
        gameObject.transform.position = spawnPosition;
        Enable();
    }

    public void TakeDamage(float damageAmount)
    {
        if (Dead)
            return;

        Health -= damageAmount;
        Health = Mathf.Clamp(Health, 0f, float.MaxValue);
        OnHealthChange?.Invoke(this, new HealthChangeArgs(StartingHealth, Health));

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