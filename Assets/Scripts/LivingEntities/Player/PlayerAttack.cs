using System;
using UnityEngine;
using UnityEngine.Events;
using AudioType = Audio.AudioType;

namespace LivingEntities.Player
{
    [Serializable]
    public abstract class PlayerAttack : MonoBehaviour
    {
        public float AttackDelay;
        public float AttackRadius;
        public float AttackTime;
        public float Damage;
        public float AttackCooldown;
        public string AnimatorPropertyString;

        protected int AnimatorProperty;
        protected Animator Animator;
        protected ContactFilter2D ContactFilter;
        protected Vector2 AttackDirection;

        public UnityEvent<float> AttackEvent;
    
        private float LastAttackTime;
        public bool CanAttack => Time.time >= LastAttackTime + AttackCooldown;
        public bool IsAttacking => Time.time <= LastAttackTime + AttackTime;

        public virtual void Init(Animator animator, ContactFilter2D contactFilter2D)
        {
            Animator = animator;
            AnimatorProperty = Animator.StringToHash(AnimatorPropertyString);
            LastAttackTime = Time.time - AttackCooldown;
            ContactFilter = contactFilter2D;
        }

        public bool TryAttack(Vector2 attackDirection)
        {
            if (!CanAttack)
                return false;
        
            LastAttackTime = Time.time;
            AttackDirection = attackDirection;
            AttackEvent?.Invoke(AttackCooldown);
            Animator.SetTrigger(AnimatorProperty);
            Attack();
            return true;
        }

        protected abstract void Attack();
        protected abstract AudioType GetAttackAudioType();

        public virtual void SetReady()
        {
            LastAttackTime = Time.time - AttackCooldown;
        }
    }
    
    public enum PlayerAttackType
    {
        Sword,
        Dash,
        Ranged
    }
}