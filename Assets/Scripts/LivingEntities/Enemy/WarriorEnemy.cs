using UnityEngine;

namespace LivingEntities.Enemy
{
    public class WarriorEnemy : Enemy
    {
        [SerializeField] private float AttackRadius = 2f;
        [SerializeField] private LayerMask PlayerMask;
    
        protected override void Attack()
        {
            base.Attack();
            var playerCollider = Physics2D.OverlapCircle(transform.position, AttackRadius, PlayerMask);
            if (playerCollider != null)
                playerCollider.GetComponent<LivingEntity>().TakeDamage(Damage);
        }
    }
}