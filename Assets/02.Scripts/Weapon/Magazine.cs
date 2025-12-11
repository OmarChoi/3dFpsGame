using System;
using UnityEngine;

[Serializable]
public class Magazine
{
    [Header("탄창 크기")]
    [Space]
    [SerializeField] private int _magazineSize = 30;
    [SerializeField] private int _totalBullet  = 120;
    [SerializeField] private int _remainBullet = 30;
    
    [Header("재장전")]
    [Space]
    [SerializeField] private float _reloadTime = 2.0f;
    
    public event Action<int> OnBulletCountChanged;
    public event Action<int> OnTotalBulletCountChanged;

    public float ReloadTime => _reloadTime;
    public bool CanReload => _remainBullet < _magazineSize && _totalBullet > 0;
    public bool HasBullet => _remainBullet > 0;

    public void Init()
    {
        _remainBullet = _magazineSize;
        OnBulletCountChanged?.Invoke(_remainBullet);
        OnTotalBulletCountChanged?.Invoke(_totalBullet);
    }
    
    public bool TryConsumeBullet()
    {
        if (_remainBullet <= 0) return false;
        
        _remainBullet--;
        OnBulletCountChanged?.Invoke(_remainBullet);
        return true;
    }
    
    public void Reload()
    {
        int neededBullet = _magazineSize - _remainBullet;
        int bulletsToReload = Mathf.Min(neededBullet, _totalBullet);
        
        _totalBullet -= bulletsToReload;
        _remainBullet += bulletsToReload;
        
        OnBulletCountChanged?.Invoke(_remainBullet);
        OnTotalBulletCountChanged?.Invoke(_totalBullet);
    }
}