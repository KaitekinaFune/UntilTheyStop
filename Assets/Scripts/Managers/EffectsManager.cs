using Effects;
using LivingEntities.Enemy;
using Pools;
using Projectiles;
using UnityEngine;

namespace Managers
{
    public class EffectsManager : MonoBehaviour
    {
        [SerializeField] private DeathEffect DeathEffectPrefab;
        [SerializeField] private SpawnEffect SpawnEffectPrefab;
        [SerializeField] private int PoolSize;
    
        private Pool<DeathEffect> DeathEffectPool;
        private Pool<SpawnEffect> SpawnEffectPool;
    
        private void Awake()
        {
            DeathEffectPool = new Pool<DeathEffect>(new PrefabFactory<DeathEffect>(DeathEffectPrefab), PoolSize);
            SpawnEffectPool = new Pool<SpawnEffect>(new PrefabFactory<SpawnEffect>(SpawnEffectPrefab), PoolSize);
        }
        
        public void OnEnemyDeath(Enemy enemy)
        {
            var pos = enemy.transform.position;
            var effect = DeathEffectPool.Allocate();

            effect.transform.position = pos;
            effect.gameObject.SetActive(true);
            effect.OnAnimationEnd += OnAnimationEnd;
            void OnAnimationEnd()
            {
                effect.OnAnimationEnd -= OnAnimationEnd;
                effect.Reset();
            }
        }
        
        public void OnEnemySpawn(Enemy enemy)
        {
            var pos = enemy.transform.position;
            var effect = SpawnEffectPool.Allocate();

            effect.transform.position = pos;
            effect.gameObject.SetActive(true);
            effect.OnAnimationEnd += OnAnimationEnd;
            void OnAnimationEnd()
            {
                effect.OnAnimationEnd -= OnAnimationEnd;
                effect.Reset();
            }
        }
    }
}
