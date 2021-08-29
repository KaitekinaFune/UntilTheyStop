using UnityEngine;
using UnityEngine.Events;
using Utils;
using Vector3 = UnityEngine.Vector3;

namespace LivingEntities.Player
{
    public class PlayerManager : Singleton<PlayerManager>
    {
        [SerializeField] private Transform PlayerSpawnPoint;
        [SerializeField] private Player Player;
        private Transform PlayerTransform;

        public UnityEvent<Player> OnPlayerDeath;
        public UnityEvent<Player> OnPlayerRespawned;
        public UnityEvent<PlayerAttackType> OnPlayerAttack;
        public UnityEvent OnPlayerTakeHit;

        protected override void Awake()
        {
            base.Awake();
            
            Player.OnDeath += InvokeOnPlayerDeath;
            Player.OnSpawn += InvokeOnPlayerRespawned;
            Player.OnTakeHit += InvokeOnPlayerTakeHit;
            Player.OnAttack += InvokeOnPlayerAttack;
            PlayerTransform = Player.transform;
        }

        public void Respawn()
        {
            Player.Spawn(PlayerSpawnPoint.position);
        }

        public void SetReady(bool value)
        {
            Player.SetReady(value);
        }
        
        private void OnDestroy()
        {
            Player.OnDeath -= InvokeOnPlayerDeath;
            Player.OnSpawn -= InvokeOnPlayerRespawned;
            Player.OnTakeHit -= InvokeOnPlayerTakeHit;
            Player.OnAttack -= InvokeOnPlayerAttack;
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

        private void InvokeOnPlayerDeath(LivingEntity entity)
        {
            OnPlayerDeath?.Invoke(Player);
        }
    
        private void InvokeOnPlayerRespawned(LivingEntity entity)
        {
            OnPlayerRespawned?.Invoke(Player);
        }

        private void InvokeOnPlayerTakeHit()
        {
            OnPlayerTakeHit?.Invoke();
        }
        
        private void InvokeOnPlayerAttack(PlayerAttackType playerAttackType)
        {
            OnPlayerAttack?.Invoke(playerAttackType);
        }
    }
}
