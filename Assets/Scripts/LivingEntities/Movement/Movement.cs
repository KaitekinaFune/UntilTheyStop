using UnityEngine;

namespace LivingEntities
{
    public class Movement
    {
        private Rigidbody2D Rb;
        private float Speed;

        public void SetRigidbody(Rigidbody2D rb)
        {
            Rb = rb;
        }

        public void SetMovementSpeed(float speed)
        {
            Speed = speed;
        }

        public void Move(Vector2 moveAmount, float movementModifier)
        {
            var speed = Speed * movementModifier;
            var move = moveAmount * speed * Time.deltaTime;
            Rb.AddForce(move, ForceMode2D.Impulse);
        }
    }
}