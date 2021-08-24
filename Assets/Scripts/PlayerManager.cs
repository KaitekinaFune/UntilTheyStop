using System.Linq;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] private Transform PlayerSpawnPoint;
    
    private Player Player;
    private Transform PlayerTransform;
    
    private void Start()
    {
        Player = PlayerPool.Instance.Get(1).FirstOrDefault();
        Player.Respawn(PlayerSpawnPoint.position);
        PlayerTransform = Player.transform;
    }

    public void DealDamageToPlayer(float damage)
    {
        Player.TakeDamage(damage);
    }

    public Vector3 GetPlayerPosition()
    {
        return PlayerTransform.position;
    }

    public Vector3 GetDirectionToPlayer(Vector3 from)
    {
        return PlayerTransform.position - from;
    }
}
