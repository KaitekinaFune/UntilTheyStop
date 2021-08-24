using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private string MenuScene;
    [SerializeField] private string GameScene;
    
    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(GameScene);
    }
}
