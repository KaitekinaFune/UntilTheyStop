using System;
using UnityEngine;
using UnityEngine.Events;

namespace LivingEntities.Player
{
    [Serializable]
    public abstract class PlayerAttack : MonoBehaviour
    {
        [SerializeField] protected float AttackDelay;
        [SerializeField] protected float AttackRadius;
        [SerializeField] protected float AttackTime;
        [SerializeField] protected float Damage;
        [SerializeField] protected float AttackCooldown;
        [SerializeField] protected string AnimatorPropertyString;

        protected int AnimatorProperty;
        protected Animator Animator;
        protected ContactFilter2D ContactFilter;
        protected Vector2 AttackDirection;
    
        private float LastAttackTime;
        private bool CanAttack => Time.time >= LastAttackTime + AttackCooldown;

        public UnityEvent<float> AttackEvent;
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

        public virtual void SetReady()
        {
            LastAttackTime = Time.time - AttackCooldown;
        }

        protected abstract void Attack();
    }
}