using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillIcon : MonoBehaviour
    {
        [SerializeField] private Image CooldownFill;

        public void OnGameReady()
        {
            CooldownFill.fillAmount = 0f;
        }

        public void StartCooldown(float cooldown)
        {
            StartCoroutine(CountCooldown(cooldown));
        }

        private IEnumerator CountCooldown(float cooldownTime)
        {
            CooldownFill.fillAmount = 1f;
        
            for (var t = 0f; t <= cooldownTime; t += Time.deltaTime)
            {
                CooldownFill.fillAmount -= 1 / cooldownTime * Time.deltaTime;
                yield return null;
            }
        }
    }
}
