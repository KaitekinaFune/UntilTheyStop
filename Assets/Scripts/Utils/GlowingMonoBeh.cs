using System.Collections;
using SpriteGlow;
using UnityEngine;

namespace Utils
{
    public class GlowingMonoBeh : MonoBehaviour
    {
        private const float GlowAlpha = 0.25f;
        private const float NoGlowAlpha = 0f;
        private SpriteGlowEffect SpriteGlow;
    
        private void Awake()
        {
            SpriteGlow = GetComponent<SpriteGlowEffect>();
        }

        public void Glow()
        {
            if (!gameObject.activeSelf)
                return;
            
            StartCoroutine(GlowAsync());
        }

        private IEnumerator GlowAsync()
        {
            float percent = 0;
            SetSpriteGlowAlpha(GlowAlpha);

            while (percent <= 1)
            {
                percent += Time.deltaTime;
                var interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
                var glowAlpha = Mathf.Lerp(NoGlowAlpha, GlowAlpha, interpolation);
                SetSpriteGlowAlpha(glowAlpha);
                yield return null;
            }
        }

        private void SetSpriteGlowAlpha(float value)
        {
            var color = SpriteGlow.GlowColor;
            color.a = value;
            SpriteGlow.GlowColor = color;
        }
    }
}