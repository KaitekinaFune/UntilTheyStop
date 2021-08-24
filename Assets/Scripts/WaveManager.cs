using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField] private Transform SpawnPoint;

    [SerializeField] private int TankEnemiesPerWave;
    [SerializeField] private int MageEnemiesPerWave;
    [SerializeField] private int ArcherEnemiesPerWave;

    private int CurrentWaveNumber;

    public void NextWave()
    {
        CurrentWaveNumber++;

        var tankEnemiesToSpawn = TankEnemiesPerWave * CurrentWaveNumber;
        var mageEnemiesPerWave = MageEnemiesPerWave * CurrentWaveNumber;
        var archerEnemiesToSpawn = ArcherEnemiesPerWave * CurrentWaveNumber;

        var tankEnemies = WarriorEnemyPool.Instance.Get(tankEnemiesToSpawn);
        var mageEnemies = MageEnemyPool.Instance.Get(mageEnemiesPerWave);
        var archerEnemies = ArcherEnemyPool.Instance.Get(archerEnemiesToSpawn);

        var spawnPosition = SpawnPoint.position;

        foreach (var enemy in tankEnemies)
        {
            enemy.Respawn(spawnPosition);
        }
        
        foreach (var enemy in mageEnemies)
        {
            enemy.Respawn(spawnPosition);
        }

        foreach (var enemy in archerEnemies)
        {
            enemy.Respawn(spawnPosition);
        }
    }
}