using System.Linq;
using UnityEngine;

public class MageEnemy : Enemy
{
    [SerializeField] private int AlliesPerHeal;

    [SerializeField] private GlowingMonoBeh StaffGlow;

    protected override bool CanAttack()
    {
        return Time.time >= NextTimeToAttack;
    }

    protected override void Attack()
    {
        base.Attack();
        var alliesToHeal = EnemiesManager.Instance.GetAlliesWithLowestHealth(AlliesPerHeal);
        var alliesList = alliesToHeal.ToList();
        if (alliesList.Count > 0)
        {
            foreach (var ally in alliesList)
                ally.Heal(Damage);
            
            StaffGlow.Glow();
        }
    }
}