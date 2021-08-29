using LivingEntities.Player;
using UnityEngine;
using Utils;

namespace LivingEntities.Enemy
{
    public abstract class Enemy : MovingLivingEntity
    {
        private static readonly int AttackAnimatorProperty = Animator.StringToHash("Attack");
    
        [SerializeField] protected float PlayerAvoidanceRadius;
        [SerializeField] protected float PlayerAvoidanceRadiusTolerance;
        [SerializeField] private float AttackRate;
        [SerializeField] protected float Damage;

        private SpriteRenderer SpriteRenderer;
        private Transform Transform;
        private EnemyStateHandler StateHandler;
        private GlowingMonoBeh SpriteGlow;
        protected float NextTimeToAttack;

        protected virtual bool CanAttack()
        {
            return Time.time >= NextTimeToAttack &&
                   (StateHandler.CurrentState == EnemyState.OnAvoidRadius ||
                    StateHandler.CurrentState == EnemyState.Near);
        }

        protected override void Awake()
        {
            base.Awake();
            Transform = transform;
            SpriteGlow = GetComponent<GlowingMonoBeh>();
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected void Start()
        {
            StateHandler = new EnemyStateHandler(Transform,
                PlayerAvoidanceRadius, PlayerAvoidanceRadiusTolerance);
        }

        public override void Heal(float healAmount)
        {
            base.Heal(healAmount);
            SpriteGlow.Glow();
        }
  
        protected override void Update()
        {
            base.Update();
            StateHandler.SetState();

            if (CanAttack())
                Attack();
        
            SetMirroring();
        }

        protected override Vector2 GetDirection()
        {
            return GetDirectionToPlayer();
        }

        private Vector2 GetDirectionToPlayer()
        {
            var direction = PlayerManager.Instance.GetDirectionToPlayer(Transform.position);
            return direction.normalized;
        }

        private void SetMirroring()
        {
            var directionToPlayer = PlayerManager.Instance.GetDirectionToPlayer(Transform.position);

            if (directionToPlayer.x < 0f)
                SpriteRenderer.flipX = true;
            else if (directionToPlayer.x >= 0f)
                SpriteRenderer.flipX = false;
        }

        protected virtual void Attack()
        {
            NextTimeToAttack = Time.time + AttackRate / 1f;
            Animator.SetTrigger(AttackAnimatorProperty);
        }

        protected override float GetMovementModifier()
        {
            switch (StateHandler.CurrentState)
            {
                case EnemyState.OnAvoidRadius:
                    return 0f;
                case EnemyState.Near:
                    return -0.35f;
                case EnemyState.Far:
                    break;
            }
        
            return base.GetMovementModifier();
        }
    }
}