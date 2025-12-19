using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<GameObject> _muzzleEffects;
    private Coroutine[] _coroutines;
    private Camera _mainCamera;
    private CameraController _cameraController;
    private Animator _animator;
    
    private void Awake()
    {
        _mainCamera = Camera.main;
        _cameraController = _mainCamera?.GetComponent<CameraController>();
        _gunWeapon.OnCoroutineRequested += StartCoroutine;
        _animator = GetComponentInChildren<Animator>();
        _coroutines = new Coroutine[_muzzleEffects.Count];
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
        if (!CursorManager.Instance.IsCursorLocked) return;
        HandleInput();
    }

    private void HandleInput()
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    private void TryFire()
    {
        Vector3 fireDirection = _mainCamera.transform.forward;
        if (_gunWeapon.TryShot(_firePosition, fireDirection, _hitEffect, _cameraController))
        {
            int muzzleIndex = UnityEngine.Random.Range(0, _muzzleEffects.Count);
            if (_coroutines[muzzleIndex] != null) return;
            _coroutines[muzzleIndex] = StartCoroutine(MuzzleFlashCoroutine(muzzleIndex));
        }
    }

    private IEnumerator MuzzleFlashCoroutine(int muzzleType = 0)
    {
        GameObject muzzleEffect = _muzzleEffects[muzzleType];
        muzzleEffect.SetActive(true);
        // Todo. 매직넘버 상수로 변경 필요
        yield return new WaitForSeconds(0.06f);
        muzzleEffect.SetActive(false);
        _coroutines[muzzleType] = null;
    }

    private void TryReload()
    {
        _gunWeapon.TryReload();
    }
}