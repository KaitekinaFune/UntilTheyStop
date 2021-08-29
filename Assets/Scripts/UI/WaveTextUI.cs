using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveTextUI : MonoBehaviour
    {
        [SerializeField] private string WaveText = "Wave: ";
        [SerializeField] private TextMeshProUGUI Text;
        
        public void SetWaveText(int number)
        {
            Text.SetText($"{WaveText}{number}");
        }

        public void OnGameOver()
        {
            Text.SetText($"{WaveText}{WaveManager.Instance.CurrentWaveNumber}");
        }
    }
}
