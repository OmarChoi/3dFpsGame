using System;
using System.Collections;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    public event Action<int> OnBulletCountChanged;
    public event Action<int> OnTotalBulletCountChanged;
    public event Action<float> OnReload;

    
    [SerializeField] private Transform _firePosition;
    [SerializeField] private ParticleSystem _hitEffect;

    [Header("발사속도")]
    [Space]
    [SerializeField] private float _fireSpeed;
    private float _lastFireTime;
    
    [Header("탄창")]
    [Space]
    [SerializeField] private int _magazineSize;
    [SerializeField] private int _totalBullet;
    [SerializeField] private float _reloadTime;
    private int _remainBullet;
    private Coroutine _reloadCoroutine;
    private void Start()
    {
        Reload();
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    private void TryReload()
    {
        if (_reloadCoroutine != null) return;
        _reloadCoroutine = StartCoroutine(ReloadCoroutine());
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

    private void Reload()
    {
        int chargingBullet = _magazineSize - _remainBullet;
        _totalBullet -= chargingBullet;
        OnTotalBulletCountChanged?.Invoke(_totalBullet);
        
        _remainBullet = _magazineSize;
        OnBulletCountChanged?.Invoke(_remainBullet);
    }

    private IEnumerator ReloadCoroutine()
    {
        float elapsedTime = 0.0f;
        OnReload?.Invoke(0.0f);
        while (elapsedTime < _reloadTime)
        {
            elapsedTime += Time.deltaTime;
            OnReload?.Invoke(elapsedTime / _reloadTime);
            yield return null;
        }
        Reload();
        OnReload?.Invoke(1.0f);
        _reloadCoroutine = null;
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
