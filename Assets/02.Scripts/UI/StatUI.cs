using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    private PlayerStat _playerStat;
    
    [Header("UI")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;

    private void Awake()
    {
        _playerStat = FindFirstObjectByType<PlayerStat>();
    }
    
    private void OnEnable()
    {
        _playerStat.OnStaminaChanged += UpdateStaminaUI;
    }

    private void OnDisable()
    {
        _playerStat.OnStaminaChanged -= UpdateStaminaUI;
    }

    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        _staminaBar.value = stamina / maxStamina;
    }
}
