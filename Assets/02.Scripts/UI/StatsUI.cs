using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    private PlayerStats _stats;
    private PlayerFire _playerFire;
    
    [Header("UI")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;
    [SerializeField] private TextMeshProUGUI _bombCount;
    private void Awake()
    {
        _stats = FindFirstObjectByType<PlayerStats>();
        _playerFire = FindFirstObjectByType<PlayerFire>();
    }

    private void Update()
    {
        UpdateStaminaUI(_stats.Stamina.Value, _stats.Stamina.MaxValue);
        UpdateBombCount();
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        _staminaBar.value = stamina / maxStamina;
    }

    private void UpdateBombCount()
    {
        int count = _playerFire.BombCount;
        _bombCount.text = $"x{count}";
    }
}
