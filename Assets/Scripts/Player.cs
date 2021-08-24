using UnityEngine;

public class Player : MovingLivingEntity
{
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int Vertical = Animator.StringToHash("Vertical");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int Attack = Animator.StringToHash("Attack");
    
    [SerializeField] private float MovingSpeed;
    [SerializeField] private float AttackRate;

    private float NextTimeToAttack;
    private bool CanAttack => Time.time >= NextTimeToAttack;
    private Vector3 Direction;
    private Vector3 LastAnimationDirection;

    protected override void Start()
    {
        base.Start();
        InputManager.Instance.OnFire1ButtonPressed += TryFirstAttack;
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnFire1ButtonPressed -= TryFirstAttack;
    }

    private void TryFirstAttack()
    {
        if (!CanAttack)
            return;

        NextTimeToAttack = Time.time + AttackRate / 1f;
        Animator.SetTrigger(Attack);
    }

    protected override void Update()
    {
        base.Update();
        var moveX = InputManager.Instance.HorizontalInput;
        var moveY = InputManager.Instance.VerticalInput;

        Direction = new Vector2(moveX, moveY).normalized;
        
        if (moveX != 0 || moveY != 0)
            LastAnimationDirection = Direction;
        
        Animator.SetBool(Idle, Direction == Vector3.zero);
        Animator.SetFloat(Horizontal, LastAnimationDirection.x);
        Animator.SetFloat(Vertical, LastAnimationDirection.y);
    }

    protected override Vector2 GetDirection()
    {
        return Direction;
    }

    protected override float GetMovementSpeed()
    {
        return MovingSpeed;
    }

    protected override void ReturnToPool()
    {
        PlayerPool.Instance.ReturnObjectToPool(this);
    }
}