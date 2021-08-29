using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] private string GameScene;

        public void LoadGameScene()
        {
            SceneManager.LoadScene(GameScene);
        }
    }
}
