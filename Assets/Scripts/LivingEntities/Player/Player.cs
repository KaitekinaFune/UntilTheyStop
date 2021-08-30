using System;
using Managers;
using UnityEngine;

namespace LivingEntities.Player
{
    public class Player : MovingLivingEntity
    {
        [SerializeField] private PlayerSwordAttack SwordAttack;
        [SerializeField] private PlayerDashAttack DashAttack;
        [SerializeField] private PlayerRangedAttack RangedAttack;
        [SerializeField] private LayerMask EnemyLayerMask;

        private PlayerMoveAnimation MoveAnimation;
    
        private Vector3 Direction;
        private Vector3 AttackDirection;
        private ContactFilter2D ContactFilter2D;
        private bool Ready;

        public event Action<PlayerAttackType> OnAttack;
        public Vector3 LookDirection => AttackDirection;

        protected void Start()
        {
            MoveAnimation = new PlayerMoveAnimation(Animator);
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
            
            MoveAnimation?.SetIdle(true);
        }

        public void TrySwordAttack()
        {
            if (!Ready || Dead)
                return;

            if (SwordAttack.TryAttack(AttackDirection))
                OnAttack?.Invoke(PlayerAttackType.Sword);
        }
    
        public void TryDashAttack()
        {
            if (!Ready || Dead)
                return;
        
            if (DashAttack.TryAttack(AttackDirection))
                OnAttack?.Invoke(PlayerAttackType.Dash);
        }
        
        public void TryRangedAttack()
        {
            if (!Ready || Dead)
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
            if (!Ready || Dead)
                return;
            
            base.Update();
            var moveX = InputManager.Instance.HorizontalInput;
            var moveY = InputManager.Instance.VerticalInput;

            Direction = new Vector2(moveX, moveY).normalized;
        
            if (moveX != 0 || moveY != 0)
                AttackDirection = Direction;

            if (IsAttacking())
                return;

            MoveAnimation?.SetIdle(Direction == Vector3.zero);
            MoveAnimation?.SetHorizontal(AttackDirection.x);
            MoveAnimation?.SetVertical(AttackDirection.y);
        }

        public void OnNewWave()
        {
            HealFull();
        }
        
        private bool IsAttacking()
        {
            return SwordAttack.IsAttacking
                   || DashAttack.IsAttacking
                   || RangedAttack.IsAttacking;
        }

        protected override Vector2 GetDirection()
        {
            return DashAttack.IsDashing ? AttackDirection : Direction;
        }

        protected override float GetMovementModifier()
        {
            if (DashAttack.IsDashing)
                return DashAttack.GetDashSpeed;

            return IsAttacking() ? 0.35f : base.GetMovementModifier();
        }
    }
}