using Pools;
using Projectiles;
using UnityEngine;
using Utils;

namespace Managers
{
    public class ProjectilesManager : Singleton<ProjectilesManager>
    {
        [SerializeField] private int PoolSize;
        [SerializeField] private Arrow ArrowPrefab;
        [SerializeField] private DamageZone DamageZonePrefab;
    
        private Pool<Arrow> ArrowPool;
        private Pool<DamageZone> DamageZonePool;

        protected override void Awake()
        {
            base.Awake();
            ArrowPool = new Pool<Arrow>(new PrefabFactory<Arrow>(ArrowPrefab), PoolSize);
            DamageZonePool = new Pool<DamageZone>(new PrefabFactory<DamageZone>(DamageZonePrefab), PoolSize);
        }

        public Arrow GetArrow()
        {
            var arrow = ArrowPool.Allocate();
            arrow.transform.parent = transform;
            return arrow;
        }

        public DamageZone GetDamageZone()
        {
            var dz = DamageZonePool.Allocate();
            dz.transform.parent = transform;
            return dz;
        }

        public void Return(Arrow arrow)
        {
            ArrowPool.Release(arrow);
        }
    
        public void Return(DamageZone arrow)
        {
            DamageZonePool.Release(arrow);
        }

        public void OnGameStart()
        {
            ArrowPool.ReturnAll();
            DamageZonePool.ReturnAll();
        }
    }
}