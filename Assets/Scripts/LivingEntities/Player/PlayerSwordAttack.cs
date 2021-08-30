using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LivingEntities.Player
{
    [Serializable]
    public class PlayerSwordAttack : PlayerAttack
    {
        protected override void Attack()
        {
            StartCoroutine(nameof(DealDamageWithDelay));
        }

        private IEnumerator DealDamageWithDelay()
        {
            yield return new WaitForSeconds(AttackDelay);
        
            var collidersList = new List<Collider2D>();
            Physics2D.OverlapCircle((Vector2)transform.position + AttackDirection/2f,
                AttackRadius, ContactFilter, collidersList);

            foreach (var livingEntity in collidersList
                .Select(collider2d => collider2d.GetComponent<LivingEntity>())
                .Where(livingEntity => livingEntity != null))
            {
                livingEntity.TakeDamage(Damage);
            }
        }

        public override void SetReady()
        {
            base.SetReady();
            
            StopCoroutine(nameof(DealDamageWithDelay));
        }
    }
}