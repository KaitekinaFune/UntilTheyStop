using System;
using System.Collections.Generic;
using LivingEntities;
using LivingEntities.Enemy;
using Pools;
using UnityEngine;

namespace Managers
{
    public class EnemyPoolHandler
    {
        private readonly int PoolSize;
        private readonly LivingEntity Prefab;
        private Pool<LivingEntity> Pool;
        
        public event Action<LivingEntity> EnemyDied;
        public event Action EnemyHit;

        public HashSet<LivingEntity> AliveEnemies { get; private set; }
        public int AliveEnemiesCount => AliveEnemies.Count;

        public EnemyPoolHandler(int poolSize, Enemy prefab)
        {
            PoolSize = poolSize;
            Prefab = prefab;
        }

        public void Init(Transform parent)
        {
            Pool = new Pool<LivingEntity>(new PrefabFactory<LivingEntity>(Prefab, parent), PoolSize);
            AliveEnemies = new HashSet<LivingEntity>();
        }

        public LivingEntity Allocate()
        {
            var enemy = Pool.Allocate();
            AliveEnemies.Add(enemy);
            enemy.OnDeath += OnEnemyDeath;
            enemy.OnTakeHit += OnEnemyHit;
            return enemy;
        }

        private void OnEnemyDeath(LivingEntity livingEntity)
        {
            livingEntity.OnTakeHit -= OnEnemyHit;
            livingEntity.OnDeath -= OnEnemyDeath;
            EnemyDied?.Invoke(livingEntity);
            AliveEnemies.Remove(livingEntity);
            Pool.Release(livingEntity);
        }

        private void OnEnemyHit()
        {
            EnemyHit?.Invoke();
        }

        public void ReturnAll()
        {
            Pool.ReturnAll();
        }
    }
}
