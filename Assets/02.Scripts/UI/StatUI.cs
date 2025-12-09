using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    private PlayerStat _stat;
    
    [Header("UI")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;

    private void Awake()
    {
        _stat = FindFirstObjectByType<PlayerStat>();
    }

    private void Update()
    {
        UpdateStaminaUI(_stat.Stamina.Value, _stat.Stamina.MaxValue);
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        _staminaBar.value = stamina / maxStamina;
    }
}
