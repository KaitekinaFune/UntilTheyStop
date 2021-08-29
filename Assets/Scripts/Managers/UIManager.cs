using UnityEngine;

namespace Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject PreGameUI;
        [SerializeField] private GameObject GameplayUI;
        [SerializeField] private GameObject GameOverUI;

        public void OnPreGameUI()
        {
            PreGameUI.SetActive(true);
            GameplayUI.SetActive(false);
            GameOverUI.SetActive(false);
        }

        public void OnGameplayUI()
        {
            PreGameUI.SetActive(false);
            GameplayUI.SetActive(true);
            GameOverUI.SetActive(false);
        }
    
        public void OnGameOverUI()
        {
            PreGameUI.SetActive(false);
            GameplayUI.SetActive(false);
            GameOverUI.SetActive(true);
        }
    }
}
