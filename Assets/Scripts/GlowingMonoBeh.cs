using System.Collections;
using SpriteGlow;
using UnityEngine;

public class GlowingMonoBeh : MonoBehaviour
{
    private readonly float GlowValue = 0.25f;
    private readonly float NoGlowValue = 0f;
    private SpriteGlowEffect SpriteGlow;
    
    private void Awake()
    {
        SpriteGlow = GetComponent<SpriteGlowEffect>();
    }

    public void Glow()
    {
        StartCoroutine(GlowAsync());
    }

    private IEnumerator GlowAsync()
    {
        float percent = 0;
        SetSpriteGlowAlpha(GlowValue);

        while (percent <= 1)
        {
            percent += Time.deltaTime;
            var interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            var glowAlpha = Mathf.Lerp(NoGlowValue, GlowValue, interpolation);
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