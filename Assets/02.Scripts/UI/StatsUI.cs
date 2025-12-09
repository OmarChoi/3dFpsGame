using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    private PlayerStats _stats;
    
    [Header("UI")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;

    private void Awake()
    {
        _stats = FindFirstObjectByType<PlayerStats>();
    }

    private void Update()
    {
        UpdateStaminaUI(_stats.Stamina.Value, _stats.Stamina.MaxValue);
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        _staminaBar.value = stamina / maxStamina;
    }
}
