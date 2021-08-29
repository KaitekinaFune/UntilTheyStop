using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject GameplayUI;
        [SerializeField] private GameObject GameOverUI;

        public void OnGameplayUI()
        {
            GameplayUI.SetActive(true);
            GameOverUI.SetActive(false);
        }
    
        public void OnGameOverUI()
        {
            GameplayUI.SetActive(false);
            GameOverUI.SetActive(true);
        }
    }
}
