using UnityEngine;

namespace LivingEntities
{
    public abstract class MovingLivingEntity : LivingEntity
    {
        [SerializeField] private float MoveSpeed;
    
        protected Animator Animator;
        private readonly Movement.Movement Movement = new Movement.Movement();

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
            Movement.SetRigidbody(GetComponent<Rigidbody2D>());
            Movement.SetMovementSpeed(MoveSpeed);
            Movement.SetActive(true);
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