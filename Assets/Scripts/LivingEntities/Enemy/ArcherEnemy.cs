using System;
using LivingEntities.Player;
using Managers;

namespace LivingEntities.Enemy
{
    public class ArcherEnemy : Enemy
    {
        protected override void Attack()
        {
            base.Attack();
        
            var from = transform.position;
            var playerPos = PlayerManager.Instance.GetPlayerPosition();
            var arrow = ProjectilesManager.Instance.GetArrow();
            arrow.Launch(from, playerPos);

            arrow.ProjectileDestroyed += OnProjectileDestroyed;
            void OnProjectileDestroyed(object sender, EventArgs args)
            {
                arrow.ProjectileDestroyed -= OnProjectileDestroyed;
                arrow.Reset();
                ProjectilesManager.Instance.Return(arrow);
            }
        }
    }
}