using UnityEngine;

namespace LivingEntities.Movement
{
    public class Movement
    {
        private Rigidbody2D Rb;
        private float Speed;

        private bool Active;

        public void SetRigidbody(Rigidbody2D rb)
        {
            Rb = rb;
        }

        public void SetMovementSpeed(float speed)
        {
            Speed = speed;
        }

        public void SetActive(bool value)
        {
            Active = value;
        }

        public void Move(Vector2 moveAmount, float movementModifier)
        {
            if (!Active)
                return;
            var speed = Speed * movementModifier;
            var move = moveAmount * speed * Time.deltaTime;
            var direction = Rb.position + move;
            Rb.AddForce(move, ForceMode2D.Impulse);
            //Rb.MovePosition(direction);
        }
    }
}