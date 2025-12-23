using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunFire : MonoBehaviour
{
    private Animator _animator;
    private Camera _mainCamera;
    private CameraController _cameraController;
    
    [Header("무기 설정")]
    [SerializeField] private GunWeapon _gunWeapon = new GunWeapon();
    public GunWeapon GunWeapon => _gunWeapon;
    
    [Header("발사 위치")]
    [SerializeField] private Transform _firePosition;
    
    [Header("이펙트")]
    [SerializeField] private ParticleSystem _hitEffect;
    [SerializeField] private List<GameObject> _muzzleEffects;
    private Coroutine[] _muzzleCoroutines;
    
    [Header("Zoom Mode")]
    [Space]
    [SerializeField] private float _zoomInFOV = 10f;
    [SerializeField] private float _normalFOV = 60f;
    [SerializeField] private GameObject _normalCrosshair;
    [SerializeField] private GameObject _zoomInCrosshair;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraController = _mainCamera?.GetComponent<CameraController>();
        _gunWeapon.OnCoroutineRequested += StartCoroutine;
        _animator = GetComponentInChildren<Animator>();
        _muzzleCoroutines = new Coroutine[_muzzleEffects.Count];
        
        _normalCrosshair.SetActive(true);
        _zoomInCrosshair.SetActive(false);
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
        if (!GameManager.Instance.CanPlay()) return;
        HandleInput();
    }

    private void HandleInput()
    {
        CheckIsFire();
        ZoomModeCheck();
        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }
    
    private void CheckIsFire()
    {
        if (Input.GetMouseButton(0))
        {
            _animator.SetBool("IsFire", true);
            TryFire();
        }
        else
        {
            _animator.SetBool("IsFire", false);
        }
    }

    private void ZoomModeCheck()
    {
        bool isZooming = Input.GetMouseButton(1);
        if (_zoomInCrosshair.activeSelf == isZooming) return;

        _normalCrosshair.SetActive(!isZooming);
        _zoomInCrosshair.SetActive(isZooming);
        _mainCamera.fieldOfView = isZooming ? _zoomInFOV : _normalFOV;
    }

    private void TryFire()
    {
        Vector3 fireDirection = _mainCamera.transform.forward;
        if (_gunWeapon.TryShot(_firePosition, fireDirection, _hitEffect, _cameraController))
        {
            int muzzleIndex = UnityEngine.Random.Range(0, _muzzleEffects.Count);
            if (_muzzleCoroutines[muzzleIndex] != null) return;
            _muzzleCoroutines[muzzleIndex] = StartCoroutine(MuzzleFlashCoroutine(muzzleIndex));
        }
    }

    private IEnumerator MuzzleFlashCoroutine(int muzzleType = 0)
    {
        GameObject muzzleEffect = _muzzleEffects[muzzleType];
        muzzleEffect.SetActive(true);
        // Todo. 매직넘버 상수로 변경 필요
        yield return new WaitForSeconds(0.06f);
        muzzleEffect.SetActive(false);
        _muzzleCoroutines[muzzleType] = null;
    }

    private void TryReload()
    {
        _gunWeapon.TryReload();
    }
}