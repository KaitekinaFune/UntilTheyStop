using LivingEntities.Player;
using UnityEngine.Events;
using Utils;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent PlayerDied;
        public UnityEvent PlayerRespawned;
    
        private void Start()
        {
            PlayerManager.Instance.OnPlayerDeath += OnPlayerDeath;
            PlayerManager.Instance.OnPlayerRespawned += OnPlayerRespawned;
            PlayerManager.Instance.Respawn();
            EnemiesManager.Instance.Begin();
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.OnPlayerDeath -= OnPlayerDeath;
            PlayerManager.Instance.OnPlayerRespawned -= OnPlayerRespawned;
        }

        public void Restart()
        {
            PlayerManager.Instance.Respawn();
            EnemiesManager.Instance.Restart();
        }

        private void OnPlayerDeath()
        {
            PlayerDied?.Invoke();
        }

        private void OnPlayerRespawned()
        {
            PlayerRespawned?.Invoke();
        }
    }
}
