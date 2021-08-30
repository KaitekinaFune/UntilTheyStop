using LivingEntities.Enemy;
using UnityEngine;

namespace Pools
{
    [CreateAssetMenu(menuName = "Enemy Pool", fileName = "New Enemy Pool Data")]
    public class EnemyPoolData : ScriptableObject
    {
        public int PoolSize;
        public Enemy Prefab;
        public EnemyType EnemyType;
    }
}