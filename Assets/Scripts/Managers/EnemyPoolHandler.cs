using System;
using System.Collections.Generic;
using LivingEntities;
using LivingEntities.Enemy;
using Pools;

namespace Managers
{
    public class EnemyPoolHandler
    {
        private readonly int PoolSize;
        private readonly Enemy Prefab;
        private Pool<Enemy> Pool;
        public event Action<EnemyPoolHandler, Enemy> EnemyDied;

        public HashSet<Enemy> AliveEnemies { get; private set; }
        public int AliveEnemiesCount => AliveEnemies.Count;

        public EnemyPoolHandler(int poolSize, Enemy prefab)
        {
            PoolSize = poolSize;
            Prefab = prefab;
        }

        public void Init()
        {
            Pool = new Pool<Enemy>(new PrefabFactory<Enemy>(Prefab), PoolSize);
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
                EnemyDied?.Invoke(this, enemy);
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
