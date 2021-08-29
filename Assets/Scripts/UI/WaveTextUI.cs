using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class WaveTextUI : MonoBehaviour
    {
        private TextMeshProUGUI Text;
        private void Awake()
        {
            Text = GetComponent<TextMeshProUGUI>();
        }

        public void OnEnable()
        {
            Text.SetText($"Wave: {EnemiesManager.Instance.Wave}");
        }
    }
}
