using UnityEngine;

public abstract class Enemy : MovingLivingEntity
{
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    [SerializeField] protected float PlayerAvoidanceRadius;
    [SerializeField] protected float PlayerAvoidanceRadiusTolerance;
    [SerializeField] private float MoveSpeed;

    private Animator Animator;
    private Transform PlayerTransform;
    private Transform Transform;

    protected override void Start()
    {
        base.Start();
        PlayerTransform = PlayerTransformHolder.Instance.PlayerTransform;
        Transform = transform;
        Animator = GetComponent<Animator>();
    }
    
    protected override float GetMovementSpeed()
    {
        return MoveSpeed;
    }
    
    protected override Vector2 GetDirection()
    {
        var direction = PlayerTransform.position - Transform.position;
        var sqrMagnitude = Vector2.SqrMagnitude(direction);

        if (sqrMagnitude - PlayerAvoidanceRadius >= PlayerAvoidanceRadiusTolerance &&
            sqrMagnitude - PlayerAvoidanceRadius < PlayerAvoidanceRadius)
        {
            OnPlayerInRange();
            return Vector2.zero;
        }
        
        if (sqrMagnitude - PlayerAvoidanceRadius < PlayerAvoidanceRadiusTolerance)
        {
            direction = -direction;
        }

        return direction.normalized;
    }

    private void OnPlayerInRange()
    {
        TryAttack();
    }

    protected virtual void TryAttack()
    {
        Animator.SetTrigger(Attack);
    }
}