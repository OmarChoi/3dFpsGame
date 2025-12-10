using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private PlayerBombFire _playerBombFire;
    [SerializeField] private PlayerGunFire _playerGunFire;
    
    [Header("UI")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;
    [SerializeField] private TextMeshProUGUI _bombCount;
    [SerializeField] private TextMeshProUGUI _bulletCount;

    private void Awake()
    {
        _stats.Stamina.OnValueChanged += UpdateStaminaUI;
        // _playerGunFire.OnBulletCountChanged += UpdateBulletCountCount;
        _playerBombFire.OnBombCountChanged += UpdateBombCount;
    }

    private void OnDisable()
    {
        _stats.Stamina.OnValueChanged -= UpdateStaminaUI;
        // _playerGunFire.OnBulletCountChanged -= UpdateBulletCountCount;
        _playerBombFire.OnBombCountChanged -= UpdateBombCount;
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        _staminaBar.value = stamina / maxStamina;
    }

    private void UpdateBombCount(int count)
    {
        _bombCount.text = $"x{count}";
    }

    private void UpdateBulletCountCount(int count, int totalCount)
    {
        _bulletCount.text = $"{count} / {totalCount}";
    }
}
