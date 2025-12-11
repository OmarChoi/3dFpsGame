using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private PlayerBombFire _playerBombFire;
    [SerializeField] private PlayerGunFire  _playerGunFire;
    
    [Header("폭탄")]
    [Space]
    [SerializeField] private TextMeshProUGUI _bombCount;
    
    [Header("총알")]
    [Space]
    [SerializeField] private TextMeshProUGUI _bulletCount;
    [SerializeField] private TextMeshProUGUI _totalBulletCount;
    
    [Header("재장전")]
    [Space]
    [SerializeField] private Slider _reloadProgressBar;
    
    private void Awake()
    {
        _playerBombFire.OnBombCountChanged += UpdateBombCount;
        _playerGunFire.GunWeapon.Magazine.OnBulletCountChanged += UpdateBulletCount;
        _playerGunFire.GunWeapon.Magazine.OnTotalBulletCountChanged += UpdateTotalBulletCount;
        _playerGunFire.GunWeapon.OnReload += UpdateReloadProgressBar;
    }

    private void OnDisable()
    {
        _playerBombFire.OnBombCountChanged -= UpdateBombCount;
        _playerGunFire.GunWeapon.Magazine.OnBulletCountChanged -= UpdateBulletCount;
        _playerGunFire.GunWeapon.Magazine.OnTotalBulletCountChanged -= UpdateTotalBulletCount;
        _playerGunFire.GunWeapon.OnReload -= UpdateReloadProgressBar;
    }
    
    private void UpdateBombCount(int count)
    {
        _bombCount.text = $"x{count}";
    }

    private void UpdateBulletCount(int count)
    {
        _bulletCount.text = $"{count}";
    }

    private void UpdateTotalBulletCount(int totalCount)
    {
        _totalBulletCount.text = $"/ {totalCount}";
    }

    private void UpdateReloadProgressBar(float amount)
    {
        amount = Mathf.Clamp01(amount);

        bool isReloading = amount < 1.0f;
        _reloadProgressBar.gameObject.SetActive(isReloading);
        _reloadProgressBar.value = amount;
    }
}
