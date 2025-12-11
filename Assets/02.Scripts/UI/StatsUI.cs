using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    
    [Header("Stats")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;

    private void OnEnable()
    {
        _stats.Stamina.OnValueChanged += UpdateStaminaUI;
        _stats.Health.OnValueChanged += UpdateHealthUI;
    }

    private void OnDisable()
    {
        _stats.Stamina.OnValueChanged -= UpdateStaminaUI;
        _stats.Health.OnValueChanged -= UpdateHealthUI;
    }
    
    private void UpdateHealthUI(float health, float maxHealth)
    {
        if (maxHealth > 0)
        {
            _healthBar.value = health / maxHealth;
        }
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        if (maxStamina > 0)
        {
            _staminaBar.value = stamina / maxStamina;
        }
    }
}
