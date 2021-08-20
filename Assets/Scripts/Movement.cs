using UnityEngine;

public class Movement
{
    private Rigidbody2D Rb;
    private float Speed;

    private bool Active;

    public void SetRigidbody(Rigidbody2D rb)
    {
        Rb = rb;
    }

    public void SetMovementSpeed(float speed)
    {
        Speed = speed;
    }

    public void SetActive(bool value)
    {
        Active = value;
    }

    public void Move(Vector2 moveAmount)
    {
        if (!Active)
            return;
        
        moveAmount *= Speed;
        var direction = Rb.position + moveAmount * Time.deltaTime;
        Rb.MovePosition(direction);
    }
}