using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Zombie : MonoBehaviour, IDamageable
{
    private EZombieState _state = EZombieState.Idle;

    [SerializeField] private GameObject _player;
    private CharacterController _characterController;
    
    [SerializeField] private float _health;
    private Vector3 _startPosition;
    
    private Coroutine _knockbackCoroutine;
    [SerializeField] private float _hitDuration = 0.3f;
    [SerializeField] private float _knockbackRate = 0.2f;
    
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
    
    private void Idle()
    {
        // Todo. Idle Animation 실행
        if (transform.IsInRange(_player.transform.position, _detectDistance))
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
        float distance = transform.GetSquaredDistance(_player.transform.position);
        if (Util.IsInRange(distance, _attackDistance))
        {
            _state = EZombieState.Attack;
        }
        else if (!Util.IsInRange(distance, _detectDistance))
        {
            _state = EZombieState.Comeback;
        }
    }

    private void Comeback()
    {
        // Todo. Run Animation 실행
        Move(_startPosition);
        if (transform.IsInRange(_startPosition, _arrivalThreshold))
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
            if (_player.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage(_damage, transform.gameObject);
                damageable.TryTakeDamage(damage);
            }
        }
    }

    private void ApplyKnockback(Damage damage)
    {
        Vector3 direction = (transform.position - damage.Attacker.transform.position);
        direction.y = 0f;
        direction.Normalize();
        if (_knockbackCoroutine != null)
        {
            StopCoroutine(_knockbackCoroutine);
        }
        _knockbackCoroutine = StartCoroutine(KnockbackCoroutine(direction, damage.Value));
    }
    
    private IEnumerator KnockbackCoroutine(Vector3 direction, float knockbackForce)
    {
        float elapsedTime = 0f;
        while (elapsedTime < _hitDuration)
        {
            elapsedTime += Time.deltaTime;
            _characterController.Move(direction * (knockbackForce * _knockbackRate * Time.deltaTime));
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
    
    public bool TryTakeDamage(Damage damage)
    {
        if (_state == EZombieState.Death || _state == EZombieState.Hit) return false;
        _health -= damage.Value;
        if (_health <= 0)
        {
            _state = EZombieState.Death;
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            _state = EZombieState.Hit;
            StartCoroutine(HitCoroutine());
            ApplyKnockback(damage);
        }
        return true;
    }
}
