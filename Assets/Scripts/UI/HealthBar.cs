using System.Collections;
using LivingEntities;
using UnityEngine;
using Utils;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] protected LivingEntity LivingEntity;
        [SerializeField] private RectTransform HealthBarFill;
        
        [SerializeField] private float FillAnimationDuration = 2f;
        [SerializeField] private bool Animated;
        
        private Vector2 StartingSize;
    
        protected virtual void Start()
        {
            StartingSize = HealthBarFill.sizeDelta;
            LivingEntity.OnHealthChange += OnHealthChange;
        }

        private void OnDestroy()
        {
            LivingEntity.OnHealthChange -= OnHealthChange;
        }

        protected virtual void OnHealthChange(object sender, HealthChangeArgs e)
        {
            if (Animated && gameObject.activeSelf)
            {
                StopCoroutine(HealAnimation(e));
                StartCoroutine(HealAnimation(e));
            }
            else
            {
                var interpolation = e.Current / e.Max;
                var width = Mathf.Lerp(0f, StartingSize.x, interpolation);
                HealthBarFill.sizeDelta = new Vector2(width, StartingSize.y);
            }
        }

        private IEnumerator HealAnimation(HealthChangeArgs e)
        {
            var t = 0f;
            
            var fillInterpolation = e.Current / e.Max;
            var from = HealthBarFill.sizeDelta.x;
            var to = Mathf.Lerp(0f, StartingSize.x, fillInterpolation);
            
            while (t <= FillAnimationDuration)
            {
                t += Time.deltaTime;
                var timeInterpolation = Mathf.Lerp(0f, FillAnimationDuration, t);
                var width = Mathf.Lerp(from, to, timeInterpolation);
                HealthBarFill.sizeDelta = new Vector2(width, StartingSize.y);
                yield return null;
            }
        }
    }
}
