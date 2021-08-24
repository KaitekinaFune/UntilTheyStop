using UnityEngine;

public class ArcherEnemy : Enemy
{
    [SerializeField] private int ArrowsPerShot;
    
    protected override void Attack()
    {
        base.Attack();
        
        var from = transform.position;
        var playerPos = PlayerManager.Instance.GetPlayerPosition();
        var arrows = ArrowsPool.Instance.Get(ArrowsPerShot);
        
        foreach (var arrow in arrows)
            arrow.Launch(from, playerPos);
    }

    protected override void ReturnToPool()
    {
        ArcherEnemyPool.Instance.ReturnObjectToPool(this);
    }
}