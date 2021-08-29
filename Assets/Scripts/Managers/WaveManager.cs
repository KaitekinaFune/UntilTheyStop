using System.Collections;
using LivingEntities.Enemy;
using UnityEngine;
using UnityEngine.Events;
using Utils;
using Random = UnityEngine.Random;

namespace Managers
{
    public class WaveManager : Singleton<WaveManager>
    {
        [SerializeField] private int WarriorEnemiesPerWave;
        [SerializeField] private int MageEnemiesPerWave;
        [SerializeField] private int ArcherEnemiesPerWave;
        [SerializeField] private float SpawnDelay;
        [SerializeField] private float SpawnRange;
        [SerializeField] private float WaveStartDelay;
        
        [SerializeField] private UnityEvent<int> WaveNumberChanged;
        [SerializeField] private UnityEvent<Enemy> EnemySpawned;

        public int CurrentWaveNumber { get; private set; }
    
        public void NextWave()
        {
            CurrentWaveNumber++;
            WaveNumberChanged?.Invoke(CurrentWaveNumber);
            StartCoroutine(WaveStartWithDelay(WaveStartDelay));
        }
        
        private IEnumerator WaveStartWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            var warrior = WarriorEnemiesPerWave * CurrentWaveNumber;
            var mageEnemiesPerWave = MageEnemiesPerWave * CurrentWaveNumber;
            var archerEnemiesToSpawn = ArcherEnemiesPerWave * CurrentWaveNumber;

            StartCoroutine(SpawnEnemies(EnemyType.Warrior, warrior,SpawnRange));
            StartCoroutine(SpawnEnemies(EnemyType.Mage, mageEnemiesPerWave,SpawnRange));
            StartCoroutine(SpawnEnemies(EnemyType.Archer, archerEnemiesToSpawn,SpawnRange));
        }

        private IEnumerator SpawnEnemies(EnemyType type, int enemiesAmount, float range)
        {
            for (var i = 0; i < enemiesAmount; i++)
            {
                var spawnPosition = Random.insideUnitCircle * range;
                var enemy = EnemiesManager.Instance.GetEnemy(type);
                enemy.SetSpawnPosition(spawnPosition);
                EnemySpawned?.Invoke(enemy);
                
                yield return new WaitForSeconds(SpawnDelay);
                enemy.Spawn();
            }
        }

        public void ResetCurrentWaveNumber()
        {
            CurrentWaveNumber = 0;
        }
    }
}