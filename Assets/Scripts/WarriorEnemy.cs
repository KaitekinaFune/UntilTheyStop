public class WarriorEnemy : Enemy
{
    protected override void Attack()
    {
        base.Attack();
        PlayerManager.Instance.DealDamageToPlayer(Damage);
    }

    protected override void ReturnToPool()
    {
        WarriorEnemyPool.Instance.ReturnObjectToPool(this);
    }
}