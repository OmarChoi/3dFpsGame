using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Zombie : MonoBehaviour, IDamageable
{
    private EZombieState _state = EZombieState.Idle;

    [SerializeField] private GameObject _player;
    private CharacterController _characterController;
    
    [SerializeField] private ConsumableStat _health;
    public ConsumableStat Health => _health;
    private Vector3 _startPosition;
    
    private Coroutine _knockbackCoroutine;
    [SerializeField] private float _hitDuration;
    [SerializeField] private float _knockbackRate;

    [Header("Move")]
    [Space]
    [SerializeField] private float _detectDistance;
    [SerializeField] private float _moveSpeed;
    private float _yVelocity;

    [Header("Patrol")]
    [Space]
    [SerializeField] private float _patrolDistance;
    private Vector3 _patrolDestination;

    [Header("Attack")]
    [Space]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackSpeed;
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
        if (GameManager.Instance.State != EGameState.Playing) return;
        ApplyGravity();
        switch (_state)
        {
            case EZombieState.Idle:
                Idle();
                break;
            
            case EZombieState.Trace:
                Trace();
                break;
            
            case EZombieState.Patrol:
                Patrol();
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
        else
        {
            OnStartPatrol();
        }
    }

    private void Rotate(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0.0f;
        if (direction.sqrMagnitude < 0.0001f) return;
        direction.Normalize();
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = targetRotation;
    }
    
    private void Move(Vector3 targetPosition)
    {
        Rotate(targetPosition);
        Vector3 direction = (targetPosition - transform.position);
        direction.y = 0.0f;
        direction.Normalize();
        
        Vector3 horizontalVelocity = direction * _moveSpeed;
        Vector3 moveVector = horizontalVelocity + (Vector3.up * _yVelocity);

        _characterController.Move(moveVector * Time.deltaTime);
    }
    
    private void ApplyGravity()
    {
        if (_characterController.isGrounded && _yVelocity < 0)
        {
            _yVelocity = -1f;
        }
        else
        {
            _yVelocity += Define.Gravity * Time.deltaTime;
        }
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
        if (transform.IsInRange(_player.transform.position, _detectDistance))
        {
            _state = EZombieState.Trace;
            return;
        }
        
        float distanceToStart = transform.GetSquaredDistance(_startPosition);
        if (Util.IsInRange(distanceToStart, _arrivalThreshold))
        {
            _state = EZombieState.Idle;
        }
        else if (Util.IsInRange(distanceToStart, _patrolDistance))
        {
            OnStartPatrol();
        }
    }

    private void OnStartPatrol()
    {
        _patrolDestination = GetRandomPositionInRange(_startPosition, _patrolDistance);
        _state = EZombieState.Patrol;
    }
    
    private void Patrol()
    {
        // Todo. Run Animation 실행
        if (transform.IsInRange(_player.transform.position, _detectDistance))
        {
            _state = EZombieState.Trace;
            return;
        }
        if (transform.IsInRange(_patrolDestination, _arrivalThreshold))
        {
            _patrolDestination = GetRandomPositionInRange(_startPosition, _patrolDistance);
        }
        Move(_patrolDestination);
    }

    private Vector3 GetRandomPositionInRange(Vector3 center, float range)
    {
        Vector2 randomPosition = UnityEngine.Random.insideUnitCircle.normalized * range;
        Vector3 nextPositon = new Vector3(randomPosition.x, 0f, randomPosition.y);
        nextPositon += center;
        return nextPositon;
    }
    
    private void Attack()
    {
        if (!transform.IsInRange(_player.transform.position, _attackDistance))
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
        _health.TryConsume(damage.Value);
        if (_health.Value <= 0)
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
