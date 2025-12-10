using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats _stats;
    [SerializeField] private PlayerBombFire _playerBombFire;
    [SerializeField] private PlayerGunFire _playerGunFire;
    
    [Header("Stats")]
    [Space]
    [SerializeField] private Slider _healthBar;
    [SerializeField] private Slider _staminaBar;
    [SerializeField] private TextMeshProUGUI _bombCount;
    
    [Header("총알")]
    [Space]
    [SerializeField] private TextMeshProUGUI _bulletCount;
    [SerializeField] private TextMeshProUGUI _totalBulletCount;

    private void Awake()
    {
        _stats.Stamina.OnValueChanged += UpdateStaminaUI;
        _playerBombFire.OnBombCountChanged += UpdateBombCount;
        _playerGunFire.OnBulletCountChanged += UpdateBulletCountCount;
        _playerGunFire.OnTotalBulletCountChanged += UpdateTotalBulletCount;
    }

    private void OnDisable()
    {
        _stats.Stamina.OnValueChanged -= UpdateStaminaUI;
        _playerBombFire.OnBombCountChanged -= UpdateBombCount;
        _playerGunFire.OnBulletCountChanged -= UpdateBulletCountCount;
        _playerGunFire.OnTotalBulletCountChanged -= UpdateTotalBulletCount;
    }
    
    private void UpdateStaminaUI(float stamina, float maxStamina)
    {
        _staminaBar.value = stamina / maxStamina;
    }

    private void UpdateBombCount(int count)
    {
        _bombCount.text = $"x{count}";
    }

    private void UpdateBulletCountCount(int count)
    {
        _bulletCount.text = $"{count}";
    }

    private void UpdateTotalBulletCount(int totalCount)
    {
        _totalBulletCount.text = $"{totalCount}";
    }
}
