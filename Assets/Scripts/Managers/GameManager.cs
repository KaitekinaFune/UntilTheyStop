using LivingEntities.Player;
using UnityEngine.Events;
using Utils;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent GameStart;
        public UnityEvent GameReady;
        public UnityEvent PlayerDied;
        public UnityEvent PlayerRespawned;
    
        private void Start()
        {
            PrestartGame();
        }

        private void PrestartGame()
        {
            GameStart?.Invoke();
            PlayerManager.Instance.SetReady(false);
            PlayerManager.Instance.Respawn();
        }

        public void OnGameReady()
        {
            GameReady?.Invoke();
            EnemiesManager.Instance.BeginWaveWithDelay();
            PlayerManager.Instance.SetReady(true);
        }

        public void Restart()
        {
            PlayerManager.Instance.Respawn();
            EnemiesManager.Instance.Restart();
        }

        public void OnPlayerDeath()
        {
            PlayerDied?.Invoke();
        }

        public void OnPlayerRespawned()
        {
            PlayerRespawned?.Invoke();
        }
    }
}
