using System;
using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    public event Action<int> OnBombCountChanged;
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float _throwPower = 15f;
    [SerializeField] private int _bombCount = 5;
    public int BombCount => _bombCount;
    
    private Camera _mainCamera;
    private void Start()
    {
        _mainCamera = Camera.main;
        OnBombCountChanged?.Invoke(_bombCount);
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _bombCount > 0)
        {
            _bombCount--;
            OnBombCountChanged?.Invoke(_bombCount);
            Bomb bomb = BombFactory.Instance.Get(_fireTransform.position);
            bomb.Launch(_mainCamera.transform.forward, _throwPower);
        }
    }
}
