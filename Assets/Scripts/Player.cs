using UnityEngine;

public class Player : MovingLivingEntity
{
    [SerializeField] private float MovingSpeed;

    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
    }

    protected override Vector2 GetDirection()
    {
        var moveX = InputManager.Instance.HorizontalInput;
        var moveY = InputManager.Instance.VerticalInput;
        return new Vector2(moveX, moveY).normalized;
    }

    protected override float GetMovementSpeed()
    {
        return MovingSpeed;
    }
}
