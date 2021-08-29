using LivingEntities.Enemy;
using UnityEngine;

namespace Managers
{
    [CreateAssetMenu(menuName = "Pools/Enemy Pool", fileName = "New Enemy Pool Data")]
    public class EnemyPoolData : ScriptableObject
    {
        public int PoolSize;
        public Enemy Prefab;
        public EnemyType EnemyType;
    }
}