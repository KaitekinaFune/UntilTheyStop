using System;
using UnityEngine;

public abstract class LivingEntity : Poolable, IDamageable
{
    [SerializeField] private float StartingHealth;
    
    private float Health;

    private bool Dead;
    public static event Action<LivingEntity> OnSpawn;
    public static event Action<LivingEntity> OnDeath;
    public event Action OnHealthChange;
    
    protected virtual void Start()
    {
        ResetHealth();
    }

    public override void Respawn(Vector3 spawnPosition)
    {
        base.Respawn(spawnPosition);
        OnSpawn?.Invoke(this);
        ResetHealth();
        Dead = false;
    }

    protected virtual void Update()
    {
        KeepInBounds();
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

    public virtual void Heal(float healAmount)
    {
        if (Dead)
            return;

        Health += healAmount;
    }

    protected virtual void Die()
    {
        Dead = true;
        ReturnToPool();
        OnDeath?.Invoke(this);
    }

    private void ResetHealth()
    {
        Health = StartingHealth;
    }

    public float GetHealthPercent()
    {
        return Health / StartingHealth;
    }
    
    public void KeepInBounds()
    {
        var screenWidth = GetScreenHalfHeightInWorldUnits() * 2 - 1f;
        var screenHeight = GetScreenHalfHeightInWorldUnits();

        var newPosition = transform.position;
        if (newPosition.x < -screenWidth)
            newPosition.x = -screenWidth;
        if (newPosition.x > screenWidth)
            newPosition.x = screenWidth;
        if (newPosition.y < -screenHeight)
            newPosition.y = -screenHeight;
        if (newPosition.y > screenHeight)
            newPosition.y = screenHeight;

        transform.position = newPosition;

    }
    
    private static float GetScreenHalfWidthInWorldUnits()
    {
        var camera = CameraHolder.Instance.Camera;
        return camera.aspect * camera.orthographicSize;
    }

    private static float GetScreenHalfHeightInWorldUnits()
    {
        var camera = CameraHolder.Instance.Camera;
        return camera.orthographicSize;
    }
}