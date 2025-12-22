using UnityEngine;

[CreateAssetMenu(fileName = "ZombieStats", menuName = "Game/Zombie/Stats", order = 0)]
public class ZombieStats : ScriptableObject
{
    [Header("Health")]
    [SerializeField] private float _maxHealth = 100f;
    public float MaxHealth => _maxHealth;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _runSpeed = 4f;
    public float MoveSpeed => _moveSpeed;
    public float RunSpeed => _runSpeed;

    [Header("Detection & Range")]
    [SerializeField] private float _detectDistance = 10f;
    [SerializeField] private float _patrolDistance = 5f;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _arrivalThreshold = 0.5f;
    public float DetectDistance => _detectDistance;
    public float PatrolDistance => _patrolDistance;
    public float AttackDistance => _attackDistance;
    public float ArrivalThreshold => _arrivalThreshold;

    [Header("Combat")]
    [SerializeField] private float _damage = 10f;
    public float Damage => _damage;

    [Header("Hit")]
    [SerializeField] private float _hitDuration = 0.5f;
    [SerializeField] private float _knockbackRate = 0.1f;
    public float HitDuration => _hitDuration;
    public float KnockbackRate => _knockbackRate;

    [Header("Animation Timing")]
    [SerializeField] private float _waitPatrolTime = 3f;
    public float WaitPatrolTime => _waitPatrolTime;

    [Header("Action")]
    [SerializeField] private float _attackInterval = 2f;
    public float AttackInterval => _attackInterval;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10f;
    public float JumpForce => _jumpForce;
}
