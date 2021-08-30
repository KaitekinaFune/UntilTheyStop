using UnityEngine;

namespace LivingEntities.Player
{
    public class PlayerMoveAnimation
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Idle = Animator.StringToHash("Idle");

        private readonly Animator Animator;

        public PlayerMoveAnimation(Animator animator)
        {
            Animator = animator;
        }

        public void SetIdle(bool value)
        {
            Animator.SetBool(Idle, value);
        }
        
        public void SetHorizontal(float value)
        {
            Animator.SetFloat(Horizontal, value);
        }

        public void SetVertical(float value)
        {
            Animator.SetFloat(Vertical, value);
        }
    }
}