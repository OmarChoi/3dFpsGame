using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    
    [Header("Stats")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;

    private void Awake()
    {
        _stats.Stamina.OnValueChanged += UpdateStaminaUI;
    }

    private void OnDisable()
    {
        _stats.Stamina.OnValueChanged -= UpdateStaminaUI;
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        if (maxStamina > 0)
        {
            _staminaBar.value = stamina / maxStamina;
        }
    }
}
