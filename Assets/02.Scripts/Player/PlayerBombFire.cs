using System;
using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    public event Action<int> OnBombCountChanged;
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float _throwPower = 15f;
    [SerializeField] private int _bombCount = 5;
    [SerializeField] private Animator _animator;
    public int BombCount => _bombCount;
    
    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
        OnBombCountChanged?.Invoke(_bombCount);
        PlayerAnimationEvent.ThrowAnimationEnd += ThrowBomb;
    }

    private void OnDisable()
    {
        PlayerAnimationEvent.ThrowAnimationEnd -= ThrowBomb;
    }
    
    private void Update()
    {
        if (!GameManager.Instance.CanPlay()) return;
        if (!CursorManager.Instance.IsCursorLocked) return;
        if (Input.GetMouseButtonDown(2) && _bombCount > 0)
        {
            _bombCount--;
            OnBombCountChanged?.Invoke(_bombCount);
            _animator.SetTrigger("FireBomb");
        }
    }

    private void ThrowBomb()
    {
        Bomb bomb = BombFactory.Instance.Get(_fireTransform.position);
        bomb.Launch(_mainCamera.transform.forward, _throwPower);
    }
}
