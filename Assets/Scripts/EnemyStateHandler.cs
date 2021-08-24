using UnityEngine;

public class EnemyStateHandler
{
    private readonly Transform Transform;
    private readonly float PlayerAvoidanceRadius;
    private readonly float PlayerAvoidanceRadiusTolerance;

    public EnemyState CurrentState { get; private set; }
    
    public EnemyStateHandler(Transform transform, float playerAvoidanceRadius, float playerAvoidanceRadiusTolerance)
    {
        Transform = transform;
        PlayerAvoidanceRadius = playerAvoidanceRadius;
        PlayerAvoidanceRadiusTolerance = playerAvoidanceRadiusTolerance;
    }

    public void SetState()
    {
        CurrentState = GetState();
    }

    private EnemyState GetState()
    {
        var direction = PlayerManager.Instance.GetDirectionToPlayer(Transform.position);
        var sqrMagnitude = Vector2.SqrMagnitude(direction);

        if (sqrMagnitude - PlayerAvoidanceRadius >= PlayerAvoidanceRadiusTolerance &&
            sqrMagnitude - PlayerAvoidanceRadius < PlayerAvoidanceRadius)
        {
            return EnemyState.OnAvoidRadius;
        }
        
        if (sqrMagnitude - PlayerAvoidanceRadius < PlayerAvoidanceRadiusTolerance)
        {
            return EnemyState.Near;
        }

        return EnemyState.Far;
    }
}