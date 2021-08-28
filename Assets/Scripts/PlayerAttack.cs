using System;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class PlayerAttack
{
    public float AttackDelay;
    public float AttackRadius;
    public float AttackTime;
    public float Damage;
    public float AttackCooldown;
    public string AnimatorPropertyString;

    protected int AnimatorProperty;
    protected Animator Animator;

    public UnityEvent<float> AttackEvent;
    
    private float LastAttackTime;
    public bool CanAttack => Time.time >= LastAttackTime + AttackCooldown;
    public bool IsAttacking => Time.time <= LastAttackTime + AttackTime;

    public virtual void Init(Animator animator)
    {
        Animator = animator;
        AnimatorProperty = Animator.StringToHash(AnimatorPropertyString);
        LastAttackTime = Time.time - AttackCooldown;
    }

    public virtual void Attack()
    {
        LastAttackTime = Time.time;
        AttackEvent?.Invoke(AttackCooldown);
        Animator.SetTrigger(AnimatorProperty);
    }
}