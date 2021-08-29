using UnityEngine.Events;
using Utils;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent GameStart;
        public UnityEvent GameReady;
    
        private void Start()
        {
            GameStart?.Invoke();
        }
        
        public void Restart()
        {
            GameStart?.Invoke();
        }
        
        public void OnGameReady()
        {
            GameReady?.Invoke();
        }
    }
}
