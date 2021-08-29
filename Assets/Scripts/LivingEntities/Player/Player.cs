using System;
using Managers;
using UnityEngine;

namespace LivingEntities.Player
{
    public class Player : MovingLivingEntity
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Idle = Animator.StringToHash("Idle");
    
        [SerializeField] private PlayerSwordAttack SwordAttack;
        [SerializeField] private PlayerDashAttack DashAttack;
        [SerializeField] private PlayerRangedAttack RangedAttack;
        [SerializeField] private LayerMask EnemyLayerMask;
    
        private Vector3 Direction;
        private Vector3 AttackDirection;
        private ContactFilter2D ContactFilter2D;
        private bool Ready;

        public event Action<PlayerAttackType> OnAttack;
        public Vector3 LookDirection => AttackDirection;

        protected void Start()
        {
            ContactFilter2D = new ContactFilter2D
            {
                layerMask = EnemyLayerMask,
                useLayerMask = true
            };

            SwordAttack.Init(Animator, ContactFilter2D);
            DashAttack.Init(Animator, ContactFilter2D);
            RangedAttack.Init(Animator, ContactFilter2D);
        }

        public void SetReady(bool value)
        {
            Ready = value;
            
            Direction = Vector3.zero;
            SwordAttack.SetReady();
            DashAttack.SetReady();
            RangedAttack.SetReady();
            Animator.SetBool(Idle, true);
        }

        public void TrySwordAttack()
        {
            if (!Ready)
                return;
            
            if (Dead)
                return;

            if (SwordAttack.TryAttack(AttackDirection))
                OnAttack?.Invoke(PlayerAttackType.Sword);
        }
    
        public void TryDashAttack()
        {
            if (!Ready)
                return;

            if (Dead)
                return;
        
            if (DashAttack.TryAttack(AttackDirection))
                OnAttack?.Invoke(PlayerAttackType.Dash);
        }
        
        public void TryRangedAttack()
        {
            if (!Ready)
                return;

            if (Dead)
                return;

            RangedAttack.TryAttack(AttackDirection);
        }

        public void OnRangedAttackSpawned()
        {
            OnAttack?.Invoke(PlayerAttackType.Ranged);
        }

        public void OnRangedAttackDealtDamage(float lifeStealAmount)
        {
            Heal(lifeStealAmount);
        }

        protected override void Die()
        {
            base.Die();
            gameObject.SetActive(false);
        }

        protected override void Update()
        {
            base.Update();

            if (!Ready)
                return;

            var moveX = InputManager.Instance.HorizontalInput;
            var moveY = InputManager.Instance.VerticalInput;

            Direction = new Vector2(moveX, moveY).normalized;
        
            if (moveX != 0 || moveY != 0)
                AttackDirection = Direction;

            if (IsAttacking())
                return;

            Animator.SetBool(Idle, Direction == Vector3.zero);
            Animator.SetFloat(Horizontal, AttackDirection.x);
            Animator.SetFloat(Vertical, AttackDirection.y);
        }

        public void OnNewWave()
        {
            HealFull();
        }
        
        private bool IsAttacking()
        {
            return SwordAttack.IsAttacking || DashAttack.IsAttacking || RangedAttack.IsAttacking;
        }

        protected override Vector2 GetDirection()
        {
            return DashAttack.IsDashing ? AttackDirection : Direction;
        }

        protected override float GetMovementModifier()
        {
            if (DashAttack.IsDashing)
                return DashAttack.DashSpeed;

            return IsAttacking() ? 0.35f : base.GetMovementModifier();
        }
    }
}