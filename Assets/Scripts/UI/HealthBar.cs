using LivingEntities;
using UnityEngine;
using Utils;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] protected LivingEntity LivingEntity;
        [SerializeField] private RectTransform HealthBarFill;
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
            var interpolation = e.Current / e.Max;
            var width = Mathf.Lerp(0f, StartingSize.x, interpolation);
            HealthBarFill.sizeDelta = new Vector2(width, StartingSize.y);
        }
    }
}
