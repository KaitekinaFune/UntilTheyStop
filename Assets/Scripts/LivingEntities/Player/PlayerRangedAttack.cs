using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Projectiles;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LivingEntities.Player
{
    [Serializable]
    public class PlayerRangedAttack : PlayerAttack
    {
        public int PuddlesAmount;
    
        protected override void Attack()
        {
            for (var i = 0; i < PuddlesAmount; i++)
                SpawnRangedAttack();
        }
    
        private void SpawnRangedAttack()
        {
            var dz = ProjectilesManager.Instance.GetDamageZone();
            var randomSpawnPoint = (Vector2)transform.position + AttackDirection * 2
                                                               + Random.insideUnitCircle;
        
            dz.Spawn(randomSpawnPoint);
            dz.OnAnimationEnd += OnAnimationEnd;
            void OnAnimationEnd()
            {
                dz.OnAnimationEnd -= OnAnimationEnd;
                dz.Reset();
                ProjectilesManager.Instance.Return(dz);
            }

            dz.OnDamageActivated += OnDamageActivated;
            void OnDamageActivated(DamageZone damageZone)
            {
                damageZone.OnDamageActivated -= OnDamageActivated;
                StartCoroutine(DealRangedDamage(damageZone));
            }
        }

        private IEnumerator DealRangedDamage(DamageZone damageZone)
        {
            var collidersList = new List<Collider2D>();
            Physics2D.OverlapCircle(damageZone.transform.position,
                AttackRadius, ContactFilter, collidersList);

            foreach (var livingEntity in collidersList
                .Select(collider2d => collider2d.GetComponent<LivingEntity>())
                .Where(livingEntity => livingEntity != null))
            {
                livingEntity.TakeDamage(Damage);
                yield return null;
            }
        }
    }
}