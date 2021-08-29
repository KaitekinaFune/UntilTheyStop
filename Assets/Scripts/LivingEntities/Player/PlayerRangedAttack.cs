using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Projectiles;
using UnityEngine;
using UnityEngine.Events;
using AudioType = Audio.AudioType;
using Random = UnityEngine.Random;

namespace LivingEntities.Player
{
    [Serializable]
    public class PlayerRangedAttack : PlayerAttack
    {
        [SerializeField] private int PuddlesAmount;
        [SerializeField] private float PuddlesSpawnDelay;
        [SerializeField] private float PuddleLifeStealRatio;

        private Coroutine SpawnCoroutine;
        private Coroutine Coroutine;

        public UnityEvent OnPuddleSpawned;
        public UnityEvent<float> OnPuddleDealtDamage;
    
        protected override void Attack()
        {
            SpawnCoroutine = StartCoroutine(SpawnRangedAttacks());
        }

        private IEnumerator SpawnRangedAttacks()
        {
            for (var i = 0; i < PuddlesAmount; i++)
            {
                OnPuddleSpawned?.Invoke();
                SpawnRangedAttack();
                yield return new WaitForSeconds(PuddlesSpawnDelay);
            }
        }

        protected override AudioType GetAttackAudioType()
        {
            return AudioType.PlayerRangedAttack;
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
                
                if (gameObject.activeSelf)
                    Coroutine = StartCoroutine(DealRangedDamage(damageZone));
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
                OnPuddleDealtDamage?.Invoke(Damage * PuddleLifeStealRatio);
                livingEntity.TakeDamage(Damage);
                yield return null;
            }
        }

        public override void SetReady()
        {
            base.SetReady();
            
            if (Coroutine != null)
                StopCoroutine(Coroutine);

            if (SpawnCoroutine != null)
                StopCoroutine(SpawnCoroutine);
        }
    }
}