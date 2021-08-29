using System.Collections.Generic;
using System.Linq;
using LivingEntities;
using LivingEntities.Enemy;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Managers
{
    public class EnemiesManager : Singleton<EnemiesManager>
    {
        [SerializeField] private List<EnemyPoolData> EnemyPoolsData;
        private Dictionary<EnemyType, EnemyPoolHandler> Pools;

        public UnityEvent<LivingEntity> EnemyDied;
        public UnityEvent EnemyHit;
        public UnityEvent WaveEnded;

        protected override void Awake()
        {
            base.Awake();
            Pools = new Dictionary<EnemyType, EnemyPoolHandler>();
        
            foreach (var pool in EnemyPoolsData)
            {
                if (Pools.ContainsKey(pool.EnemyType))
                    Debug.LogError($"Duplicate EnemyType:{pool.EnemyType}");

                Pools.Add(pool.EnemyType, new EnemyPoolHandler(pool.PoolSize, pool.Prefab));
            }

            foreach (var pool in Pools.Values)
            {
                pool.Init(transform);
                pool.EnemyDied += OnEnemyDied;
                pool.EnemyHit += OnEnemyHit;
            }
        }

        private void OnDestroy()
        {
            foreach (var pool in Pools.Values)
            {
                pool.EnemyDied -= OnEnemyDied;
                pool.EnemyHit -= OnEnemyHit;
            }
        }

        private void OnEnemyDied(LivingEntity enemy)
        {
            EnemyDied?.Invoke(enemy);

            if (AliveEnemiesCount() <= 1)
                WaveEnded?.Invoke();
        }
        
        private void OnEnemyHit()
        {
            EnemyHit?.Invoke();
        }
        
        public void ResetEnemies()
        {
            foreach (var pool in Pools.Values)
                pool.ReturnAll();
        }

        private int AliveEnemiesCount()
        {
            return Pools.Values.Sum(pool => pool.AliveEnemiesCount);
        }
        
        public LivingEntity GetEnemy(EnemyType type)
        {
            return Pools[type].Allocate();
        }

        public IEnumerable<LivingEntity> GetAlliesWithLowestHealth(int alliesPerHeal)
        {
            var allies = new List<LivingEntity>();
            foreach (var pool in Pools.Values)
                allies.AddRange(pool.AliveEnemies.Where(aliveEnemy => aliveEnemy.NeedHealing));

            var enemies = allies.OrderBy(t => t.GetHealthPercent()).ToList();
            return enemies.Count >= alliesPerHeal ? enemies.Take(alliesPerHeal) : enemies;
        }
    }
}