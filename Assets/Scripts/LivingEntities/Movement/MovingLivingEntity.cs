using UnityEngine;

namespace LivingEntities
{
    public abstract class MovingLivingEntity : LivingEntity
    {
        [SerializeField] private float MoveSpeed;
    
        protected Animator Animator;
        private readonly Movement Movement = new Movement();

        protected override void Awake()
        {
            base.Awake();
            Animator = GetComponent<Animator>();
            Movement.SetRigidbody(GetComponent<Rigidbody2D>());
            Movement.SetMovementSpeed(MoveSpeed);
        }

        protected virtual void Update()
        {
            var direction = GetDirection();
            Movement.Move(direction, GetMovementModifier());
        }
    
        protected abstract Vector2 GetDirection();

        protected virtual float GetMovementModifier()
        {
            return 1f;
        }
    }
}