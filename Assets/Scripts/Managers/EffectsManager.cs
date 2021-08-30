using Effects;
using LivingEntities;
using Pools;
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
            var managerTransform = transform;
            DeathEffectPool = new Pool<DeathEffect>(
                new PrefabFactory<DeathEffect>(DeathEffectPrefab, managerTransform),
                PoolSize);
            
            SpawnEffectPool = new Pool<SpawnEffect>(
                new PrefabFactory<SpawnEffect>(SpawnEffectPrefab, managerTransform),
                PoolSize);
        }
        
        public void OnLivingEntityDeath(LivingEntity entity)
        {
            var pos = entity.transform.position;
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
        
        public void OnLivingEntitySpawn(LivingEntity entity)
        {
            var pos = entity.transform.position;
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
