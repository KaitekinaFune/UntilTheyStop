using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerManager : Singleton<PlayerManager>
{
    private Player Player;
    private Transform PlayerTransform;

    protected override void Awake()
    {
        base.Awake();
        Player = FindObjectOfType<Player>();
        PlayerTransform = Player.transform;
    }

    public Vector3 GetPlayerPosition()
    {
        return PlayerTransform.position;
    }

    public Vector2 GetPlayerLookDirection()
    {
        return Player.LookDirection;
    }

    public Vector3 GetDirectionToPlayer(Vector3 from)
    {
        return PlayerTransform.position - from;
    }
}
