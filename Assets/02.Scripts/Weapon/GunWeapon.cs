using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class GunWeapon
{
    [Header("발사속도")]
    [Space]
    [SerializeField] private float _fireSpeed = 10f;
    private float _lastFireTime = -999f;

    [Header("반동")]
    [Space]    
    [SerializeField] private float _minRecoilStrengthX = -0.5f;
    [SerializeField] private float _maxRecoilStrengthX = 0.5f;
    [SerializeField] private float _recoilStrengthY = 1f;
    
    [Header("탄창")]
    [Space]
    [SerializeField] private Magazine _magazine = new Magazine();
    public Magazine Magazine => _magazine;
    
    private bool _isReloading = false;

    public event Func<IEnumerator, Coroutine> OnCoroutineRequested;
    public event Action<float> OnReload;

    public void Init()
    {
        _magazine.Init();
    }

    public bool TryShot(Vector3 position, Vector3 direction, ParticleSystem hitEffect, Camera shooterCamera)
    {
        if (!CanShot()) return false;
        
        Shot(position, direction, hitEffect, shooterCamera);
        return true;
    }

    public void TryReload()
    {
        if (!_magazine.CanReload || _isReloading) return;
        OnCoroutineRequested?.Invoke(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        OnReload?.Invoke(0f);

        float elapsedTime = 0f;
        float reloadTime = _magazine.ReloadTime;

        while (elapsedTime < reloadTime)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / reloadTime);
            OnReload?.Invoke(progress);
            yield return null;
        }

        _magazine.Reload();
        _isReloading = false;
        
        OnReload?.Invoke(1f);
    }

    private bool CanShot()
    {
        if (_isReloading) return false;
        if (!_magazine.HasBullet) return false;
        if (_fireSpeed <= 0) return false;
        
        float currentTime = Time.time;
        float duration = currentTime - _lastFireTime;
        float cooldown = 1f / _fireSpeed;
        
        return duration >= cooldown;
    }
    
    private void Shot(Vector3 position, Vector3 direction, ParticleSystem hitEffect, Camera shooterCamera)
    {
        if (!_magazine.TryConsumeBullet()) return;
        
        _lastFireTime = Time.time;
        
        Ray ray = new Ray(position, direction);
        RaycastHit hitInfo = new RaycastHit();
        bool isHit = Physics.Raycast(ray, out hitInfo);
        if (isHit)
        {
            hitEffect.transform.position = hitInfo.point;
            hitEffect.transform.forward = hitInfo.normal;
            hitEffect.Play();
        }
        
        ApplyRecoil(shooterCamera);
    }

    private void ApplyRecoil(Camera shooterCamera)
    {
        if (shooterCamera == null) return;
        
        float recoilX = UnityEngine.Random.Range(_minRecoilStrengthX, _maxRecoilStrengthX);
        var cameraController = shooterCamera.GetComponent<CameraController>();
        cameraController?.CurrentCamera.AddRecoil(_recoilStrengthY, recoilX);
    }
}
