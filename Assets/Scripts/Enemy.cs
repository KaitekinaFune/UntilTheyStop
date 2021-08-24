using UnityEngine;

public abstract class Enemy : MovingLivingEntity
{
    private static readonly int AttackAnimatorProperty = Animator.StringToHash("Attack");
    
    [SerializeField] protected float PlayerAvoidanceRadius;
    [SerializeField] protected float PlayerAvoidanceRadiusTolerance;
    [SerializeField] private float MoveSpeed;
    [SerializeField] private float AttackRate;
    [SerializeField] protected float Damage;
    
    private Transform Transform;
    private EnemyStateHandler StateHandler;
    private GlowingMonoBeh SpriteGlow;
    protected float NextTimeToAttack;

    protected virtual bool CanAttack()
    {
        return Time.time >= NextTimeToAttack &&
               (StateHandler.CurrentState == EnemyState.OnAvoidRadius ||
                StateHandler.CurrentState == EnemyState.Near);
    }

    protected override void Start()
    {
        base.Start();
        Transform = transform;
        StateHandler = new EnemyStateHandler(Transform,
            PlayerAvoidanceRadius, PlayerAvoidanceRadiusTolerance);
        SpriteGlow = GetComponent<GlowingMonoBeh>();
    }

    protected override float GetMovementSpeed()
    {
        return MoveSpeed;
    }

    public override void Heal(float healAmount)
    {
        base.Heal(healAmount);
        SpriteGlow.Glow();
    }

    public override void Respawn(Vector3 spawnPosition)
    {
        base.Respawn(GetRandomSpawnPoint());
    }

    private Vector3 GetRandomSpawnPoint()
    {
        var playerPosition = PlayerManager.Instance.GetPlayerPosition();
        var halfWidthScreen = GetScreenHalfWidthInWorldUnits();
        var halfHeightScreen = GetScreenHalfHeightInWorldUnits();

        var randomXPoint = Random.Range(-halfWidthScreen, halfWidthScreen);
        var randomYPoint = Random.Range(-halfHeightScreen, halfHeightScreen);

        if (Random.value > .5f)
        {
            if (Random.value > .5f)
                return new Vector2(randomXPoint, halfHeightScreen);
                
            return new Vector2(randomXPoint, -halfHeightScreen);
        }

        if (Random.value > .5f)
        {
            return new Vector2(halfWidthScreen, randomYPoint);
        }
            
        return new Vector2(-halfWidthScreen, randomYPoint);
    }

    private static float GetScreenHalfWidthInWorldUnits()
    {
        var camera = CameraHolder.Instance.Camera;
        return camera.aspect * camera.orthographicSize;
    }

    private static float GetScreenHalfHeightInWorldUnits()
    {
        var camera = CameraHolder.Instance.Camera;
        return camera.orthographicSize;
    }

    protected override void Update()
    {
        base.Update();
        StateHandler.SetState();

        if (CanAttack())
            Attack();
    }

    protected override Vector2 GetDirection()
    {
        return GetDirectionToPlayer();
    }

    private Vector2 GetDirectionToPlayer()
    {
        var direction = PlayerManager.Instance.GetDirectionToPlayer(Transform.position);
        switch (StateHandler.CurrentState)
        {
            case EnemyState.OnAvoidRadius:
                return Vector2.zero;
            case EnemyState.Near:
                direction = -direction;
                break;
            case EnemyState.Far:
                break;
        }

        return direction.normalized;
    }

    protected override void SetMirroring()
    {
        var directionToPlayer = PlayerManager.Instance.GetDirectionToPlayer(Transform.position);
        
        if (directionToPlayer.x < 0f)
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        else if (directionToPlayer.x >= 0f)
            transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    protected virtual void Attack()
    {
        NextTimeToAttack = Time.time + AttackRate / 1f;
        Animator.SetTrigger(AttackAnimatorProperty);
    }
}