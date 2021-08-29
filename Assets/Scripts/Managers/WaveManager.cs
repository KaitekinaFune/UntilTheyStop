using System;
using LivingEntities.Enemy;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    [Serializable]
    public class WaveManager
    {
        [SerializeField] private int TankEnemiesPerWave;
        [SerializeField] private int MageEnemiesPerWave;
        [SerializeField] private int ArcherEnemiesPerWave;

        public int CurrentWaveNumber { get; private set; }
    
        public void NextWave()
        {
            CurrentWaveNumber++;

            var tankEnemiesToSpawn = TankEnemiesPerWave * CurrentWaveNumber;
            var mageEnemiesPerWave = MageEnemiesPerWave * CurrentWaveNumber;
            var archerEnemiesToSpawn = ArcherEnemiesPerWave * CurrentWaveNumber;
        
            for (var i = 0; i < tankEnemiesToSpawn; i++)
            {
                var spawnPosition = Random.insideUnitCircle * 3f;
                var enemy = EnemiesManager.Instance.GetEnemy(EnemyType.Warrior);
                enemy.Spawn(spawnPosition);
            }
        
            for (var i = 0; i < mageEnemiesPerWave; i++)
            {
                var spawnPosition = Random.insideUnitCircle * 3f;
                var enemy = EnemiesManager.Instance.GetEnemy(EnemyType.Mage);
                enemy.Spawn(spawnPosition);
            }
        
            for (var i = 0; i < archerEnemiesToSpawn; i++)
            {
                var spawnPosition = Random.insideUnitCircle * 3f;
                var enemy = EnemiesManager.Instance.GetEnemy(EnemyType.Archer);
                enemy.Spawn(spawnPosition);
            }
        }

        public void Start()
        {
            CurrentWaveNumber = 0;
            NextWave();
        }
    }
}