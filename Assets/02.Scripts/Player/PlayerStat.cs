using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerStat : MonoBehaviour
{
    [SerializeField] private ConsumableStat _stamina;
    public ConsumableStat Stamina => _stamina;
    [SerializeField] private ConsumableStat _health;
    public ConsumableStat Health => _health;

    [SerializeField] private ValueStat _damage;
    public float Damage => _damage.Value;
    [SerializeField] private ValueStat _moveSpeed;
    public float MoveSpeed => _moveSpeed.Value;
    [SerializeField] private ValueStat _runSpeed;
    public float RunSpeed => _runSpeed.Value;
    [SerializeField] private ValueStat _jumpPower;
    public float JumpPower => _jumpPower.Value;
    
    private void Start()
    {
        _health.Initialize();
        _stamina.Initialize();
    }

    private void Update()
    {
        float deltaTime = Time.deltaTime;
        
        _health.Regenerate(deltaTime);
        _stamina.Regenerate(deltaTime);
    }
}
