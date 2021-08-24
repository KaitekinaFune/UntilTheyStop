using System.Collections.Generic;
using System.Linq;

public class EnemiesManager : Singleton<EnemiesManager>
{
    private readonly HashSet<LivingEntity> Enemies = new HashSet<LivingEntity>();

    private void Start()
    {
        LivingEntity.OnSpawn += OnEnemySpawn;
        LivingEntity.OnDeath += OnEnemyDeath;
        
        WaveManager.Instance.NextWave();
    }

    private void OnDisable()
    {
        LivingEntity.OnSpawn -= OnEnemySpawn;
        LivingEntity.OnDeath -= OnEnemyDeath;
    }

    private void OnEnemyDeath(LivingEntity enemy)
    {
        if (!(enemy is Enemy))
            return;

        Enemies.Remove(enemy);
        
        if (Enemies.Count == 0)
            WaveManager.Instance.NextWave();            
    }

    private void OnEnemySpawn(LivingEntity enemy)
    {
        if (!(enemy is Enemy))
            return;
        
        Enemies.Add(enemy);
    }

    public IEnumerable<LivingEntity> GetAlliesWithLowestHealth(int alliesPerHeal)
    {
        var enemies = Enemies.OrderBy(t => t.GetHealthPercent());
        return enemies.Take(alliesPerHeal);
    }
}