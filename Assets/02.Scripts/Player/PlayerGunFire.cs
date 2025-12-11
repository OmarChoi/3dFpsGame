using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    [Header("무기 설정")]
    [SerializeField] private GunWeapon _gunWeapon = new GunWeapon();
    public GunWeapon GunWeapon => _gunWeapon;
    
    [Header("발사 위치")]
    [SerializeField] private Transform _firePosition;
    
    [Header("이펙트")]
    [SerializeField] private ParticleSystem _hitEffect;
    
    private Camera _mainCamera;
    private CameraController _cameraController;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraController = _mainCamera?.GetComponent<CameraController>();
        _gunWeapon.OnCoroutineRequested += StartCoroutine;
    }

    private void OnDestroy()
    {
        _gunWeapon.OnCoroutineRequested -= StartCoroutine;
    }

    private void Start()
    {
        _gunWeapon.Init();
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            TryFire();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    private void TryFire()
    {
        Vector3 fireDirection = _mainCamera.transform.forward;
        _gunWeapon.TryShot(_firePosition, fireDirection, _hitEffect, _cameraController);
    }

    private void TryReload()
    {
        _gunWeapon.TryReload();
    }
}