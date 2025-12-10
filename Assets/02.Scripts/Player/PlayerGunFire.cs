using System;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    public  event Action<int> OnBulletCountChanged;
    public  event Action<int> OnTotalBulletCountChanged;

    
    [SerializeField] private Transform _firePosition;
    [SerializeField] private ParticleSystem _hitEffect;

    [Header("발사속도")]
    [Space]
    [SerializeField] private float _fireSpeed;
    private float _lastFireTime;
    
    [Header("탄창")]
    [Space]
    private int _remainBullet;
    [SerializeField] private int _magazineSize;
    [SerializeField] private int _totalBullet;
    [SerializeField] private float _reloadTime;

    private void Awake()
    {
        _remainBullet = _magazineSize;
        OnBulletCountChanged?.Invoke(_remainBullet);
    }
    
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (CanShot() && CheckBullet())
            {
                Shot();
            }
        }
    }

    private bool CanShot()
    {
        if (_fireSpeed <= 0) return false;
        
        float currentTime = Time.time;
        float duration = currentTime - _lastFireTime;
        float cooldown = 1 / _fireSpeed;
        return duration > cooldown;
    }

    private bool CheckBullet()
    {
        if (_remainBullet <= 0) return false;
        _remainBullet--;
        OnBulletCountChanged?.Invoke(_remainBullet);
        return true;
    }
    
    private void Shot()
    {
        _lastFireTime = Time.time;
        Ray ray = new Ray(_firePosition.position, Camera.main.transform.forward);
        RaycastHit hitInfo = new RaycastHit();
        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit)
        {
            _hitEffect.transform.position = hitInfo.point;
            _hitEffect.transform.forward = hitInfo.normal;
            _hitEffect.Play();
        }
    }
    
}
