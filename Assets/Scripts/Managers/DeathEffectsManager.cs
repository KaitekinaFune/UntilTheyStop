using LivingEntities.Enemy;
using Pools;
using Projectiles;
using UnityEngine;

namespace Managers
{
    public class DeathEffectsManager : MonoBehaviour
    {
        [SerializeField] private DeathEffect Prefab;
        [SerializeField] private int PoolSize;
    
        private Pool<DeathEffect> Pool;
    
        private void Awake()
        {
            Pool = new Pool<DeathEffect>(new PrefabFactory<DeathEffect>(Prefab), PoolSize);
        }

        private void OnEnable()
        {
            Enemy.OnAnyEnemyDeath += OnAnyEnemyDeath;
        }

        private void OnDisable()
        {
            Enemy.OnAnyEnemyDeath -= OnAnyEnemyDeath;
        }

        private void OnAnyEnemyDeath(Enemy enemy)
        {
            var pos = enemy.transform.position;
            var effect = Pool.Allocate();

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
