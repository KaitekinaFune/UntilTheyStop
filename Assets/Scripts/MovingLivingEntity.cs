using UnityEngine;

public abstract class MovingLivingEntity : LivingEntity
{
    private readonly Movement Movement = new Movement();

    protected override void Start()
    {
        base.Start();
        Movement.SetRigidbody(GetComponent<Rigidbody2D>());
        Movement.SetMovementSpeed(GetMovementSpeed());
        Movement.SetActive(true);
    }

    protected virtual void Update()
    {
        var direction = GetDirection();
        Movement.Move(direction);
    }

    protected abstract Vector2 GetDirection();
    protected abstract float GetMovementSpeed();
}