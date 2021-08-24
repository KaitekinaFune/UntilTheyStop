using UnityEngine;

public abstract class MovingLivingEntity : LivingEntity
{
    private static readonly int MoveSpeed = Animator.StringToHash("MoveSpeed");
    
    protected Animator Animator;
    private readonly Movement Movement = new Movement();
    private bool MovementEnabled;

    protected override void Start()
    {
        base.Start();
        Animator = GetComponent<Animator>();
        Movement.SetRigidbody(GetComponent<Rigidbody2D>());
        Movement.SetMovementSpeed(GetMovementSpeed());
        Movement.SetActive(true);
    }

    public override void Respawn(Vector3 spawnPosition)
    {
        base.Respawn(spawnPosition);
        MovementEnabled = true;
    }

    protected override void Update()
    {
        base.Update();
        if (!MovementEnabled)
            return;

        var direction = GetDirection();
        Movement.Move(direction);
    }
    
    private void SetAnimatorMoveSpeed(Vector2 direction)
    {
        Animator.SetFloat(MoveSpeed, Mathf.Abs(direction.sqrMagnitude));
    }

    protected Vector3 GetVelocity()
    {
        return Movement.GetVelocity();
    }

    protected virtual void SetMirroring()
    {
        
    }
    
    protected abstract Vector2 GetDirection();
    protected abstract float GetMovementSpeed();
}