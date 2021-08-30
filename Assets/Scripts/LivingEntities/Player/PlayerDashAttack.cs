using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LivingEntities.Player
{
    [Serializable]
    public class PlayerDashAttack : PlayerAttack
    {
        [SerializeField] private float DashSpeed;
        [SerializeField] private float DashTickTime;

        public float GetDashSpeed => DashSpeed;
        public bool IsDashing { get; private set; }

        protected override void Attack()
        {
            StartCoroutine(nameof(DashCoroutine));
            StartCoroutine(nameof(DealDashDamage));
        }

        private IEnumerator DashCoroutine()
        {
            yield return new WaitForSeconds(AttackDelay);
            IsDashing = true;
            yield return new WaitForSeconds(AttackTime);
            IsDashing = false;
        }

        private IEnumerator DealDashDamage()
        {
            yield return new WaitForSeconds(AttackDelay);
            var t = 0f;
            for (; t < AttackTime; )
            {
                t += DashTickTime;
                yield return new WaitForSeconds(DashTickTime);
                
                var collidersList = new List<Collider2D>();
                Physics2D.OverlapCircle((Vector2) transform.position + (Vector2) AttackDirection,
                    AttackRadius, ContactFilter, collidersList);

                foreach (var livingEntity in collidersList
                    .Select(collider2d => collider2d.GetComponent<LivingEntity>())
                    .Where(livingEntity => livingEntity != null))
                {
                    livingEntity.TakeDamage(Damage);
                }
            }
        }

        public override void SetReady()
        {
            base.SetReady();
            
            StopCoroutine(nameof(DashCoroutine));
            StopCoroutine(nameof(DealDashDamage));
        }
    }
}