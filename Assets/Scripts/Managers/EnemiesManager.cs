using System.Collections.Generic;
using System.Linq;
using LivingEntities;
using LivingEntities.Enemy;
using UnityEngine;
using Utils;

namespace Managers
{
    public class EnemiesManager : Singleton<EnemiesManager>
    {
        [SerializeField] private List<EnemyPoolData> EnemyPoolsData;
        [SerializeField] private WaveManager WaveManager;
        private Dictionary<EnemyType, EnemyPoolHandler> Pools;

        public int Wave => WaveManager.CurrentWaveNumber;

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
                pool.Init();
                pool.EnemyDied += OnEnemyDied;
            }
        }
    
        private void OnEnemyDied(EnemyPoolHandler pool, Enemy enemy)
        {
            if (AliveEnemiesCount() <= 1)
                WaveManager.NextWave();
        }

        private int AliveEnemiesCount()
        {
            return Pools.Values.Sum(pool => pool.AliveEnemiesCount);
        }

        public void Begin()
        {
            WaveManager.Start();
        }
    
        public void Restart()
        {
            foreach (var pool in Pools.Values)
                pool.ReturnAll();
        
            WaveManager.Start();
        }

        public Enemy GetEnemy(EnemyType type)
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