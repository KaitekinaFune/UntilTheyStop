using TMPro;
using UnityEngine;

public class HealthBarText : HealthBar
{
    [SerializeField] private TextMeshProUGUI HealthText;

    protected override void Start()
    {
        base.Start();
        HealthText.SetText($"{LivingEntity.Health} / {LivingEntity.Health}");
    }

    protected override void OnHealthChange(object sender, HealthChangeArgs e)
    {
        base.OnHealthChange(sender, e);
        HealthText.SetText($"{e.Current} / {e.Max}");
    }
}