using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Zombie : MonoBehaviour
{
    private EZombieState _state = EZombieState.Idle;

    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerStats _playerStats;
    private CharacterController _characterController;
    
    [SerializeField] private float _health;
    private Vector3 _startPosition;
    
    private Coroutine _knockbackCoroutine;
    [SerializeField] private float _hitDuration = 0.3f;
    
    [Header("Move")]
    [Space]
    [SerializeField] private float _detectDistance = 4f;
    [SerializeField] private float _moveSpeed = 5.0f;
    
    [Header("Attack")]
    [Space]
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _damage = 20.0f;
    [SerializeField] private float _attackSpeed = 2.0f;
    private float _attackTimer;
    
    [SerializeField] private float _arrivalThreshold;
    [SerializeField] private float _deathDuration;

    private void Awake()
    {
        _startPosition = transform.position;
        _attackTimer = _attackSpeed;
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        switch (_state)
        {
            case EZombieState.Idle:
                Idle();
                break;
            
            case EZombieState.Trace:
                Trace();
                break;
            
            case EZombieState.Comeback:
                Comeback();
                break;
            
            case EZombieState.Attack:
                Attack();
                break;
            
            default:
                break;
        }
    }

    private bool IsInRange(Vector3 targetPosition, float range)
    {
        return (targetPosition - transform.position).sqrMagnitude <= range * range;
    }
    
    private void Idle()
    {
        // Todo. Idle Animation 실행
        if (IsInRange(_player.transform.position, _detectDistance))
        {
            _state = EZombieState.Trace;
        }
    }

    private void Move(Vector3 targetPosition)
    {        
        Vector3 direction = (targetPosition - transform.position).normalized;
        _characterController.Move(direction * (_moveSpeed * Time.deltaTime));
    }
    
    private void Trace()
    {
        // Todo. Run Animation 실행
        Move(_player.transform.position);
        if (IsInRange(_player.transform.position, _attackDistance))
        {
            _state = EZombieState.Attack;
        }
        else if (!IsInRange(_player.transform.position, _detectDistance))
        {
            _state = EZombieState.Comeback;
        }
    }

    private void Comeback()
    {
        // Todo. Run Animation 실행
        Move(_startPosition);
        if (IsInRange(_startPosition, _arrivalThreshold))
        {
            _state = EZombieState.Idle;
        }
    }

    private void Attack()
    {
        if (!IsInRange(_player.transform.position, _attackDistance))
        {
            _state = EZombieState.Trace;
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            _attackTimer = 0f;
            _playerStats?.Health.Decrease(_damage);
        }
    }

    public void ApplyKnockback(Vector3 hitPoint, float knockbackForce)
    {
        Vector3 direction = (transform.position - hitPoint);
        direction.y = 0f;
        direction.Normalize();
        if (_knockbackCoroutine != null)
        {
            StopCoroutine(_knockbackCoroutine);
        }
        _knockbackCoroutine = StartCoroutine(KnockbackCoroutine(direction, knockbackForce));
    }
    
    public bool TryTakeDamage(float damage, Vector3 hitPoint, float knockbackForce)
    {
        if (_state == EZombieState.Death || _state == EZombieState.Hit) return false;
        _health -= damage;
        if (_health <= 0)
        {
            _state = EZombieState.Death;
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            _state = EZombieState.Hit;
            StartCoroutine(HitCoroutine());
            ApplyKnockback(hitPoint, knockbackForce);
        }
        return true;
    }

    private IEnumerator KnockbackCoroutine(Vector3 direction, float knockbackForce)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _hitDuration)
        {
            elapsedTime += Time.deltaTime;
            _characterController.Move(direction * (knockbackForce * Time.deltaTime));
            yield return null;
        }
    }
    
    private IEnumerator HitCoroutine()
    {
        // Todo. Hit Animation 실행
        yield return new WaitForSeconds(_hitDuration);
        _state = EZombieState.Idle;
    }

    private IEnumerator DeathCoroutine()
    {
        // Todo. Death Animation 실행
        yield return new WaitForSeconds(_deathDuration);
        Destroy(gameObject);
    }
}
