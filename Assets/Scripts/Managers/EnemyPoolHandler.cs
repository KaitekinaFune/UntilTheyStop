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
        private readonly Enemy Prefab;
        private Pool<Enemy> Pool;
        
        public event Action<Enemy> EnemyDied;

        public HashSet<Enemy> AliveEnemies { get; private set; }
        public int AliveEnemiesCount => AliveEnemies.Count;

        public EnemyPoolHandler(int poolSize, Enemy prefab)
        {
            PoolSize = poolSize;
            Prefab = prefab;
        }

        public void Init(Transform parent)
        {
            Pool = new Pool<Enemy>(new PrefabFactory<Enemy>(Prefab, parent), PoolSize);
            AliveEnemies = new HashSet<Enemy>();
        }

        public Enemy Allocate()
        {
            var enemy = Pool.Allocate();
            AliveEnemies.Add(enemy);
            enemy.OnDeath += OnEnemyDeath;
            return enemy;
        }

        private void OnEnemyDeath(LivingEntity livingEntity)
        {
            if (livingEntity is Enemy enemy)
            {
                enemy.OnDeath -= OnEnemyDeath;
                EnemyDied?.Invoke(enemy);
                AliveEnemies.Remove(enemy);
                Pool.Release(enemy);
            }
        }

        public void ReturnAll()
        {
            Pool.ReturnAll();
        }
    }
}
