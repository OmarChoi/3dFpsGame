using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour, IDamageable
{
    private EZombieState _state = EZombieState.Idle;

    [SerializeField] private GameObject _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;

    [SerializeField] private ConsumableStat _health;
    public ConsumableStat Health => _health;
    private Vector3 _startPosition;

    private Coroutine _knockbackCoroutine;
    [SerializeField] private float _hitDuration;
    [SerializeField] private float _knockbackRate;

    [Header("Animation")]
    [Space]
    [SerializeField] private float _waitPatrolTime;
    private float _enterIdleTime;
    
    [Header("Move")]
    [Space]
    [SerializeField] private float _detectDistance;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _runSpeed;

    [Header("Patrol")]
    [Space]
    [SerializeField] private float _patrolDistance;
    private Vector3 _patrolDestination;

    [Header("Attack")]
    [Space]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackInterval;
    [SerializeField] private float _damage;
    public float Damage => _damage;
    private float _attackTimer;
    private bool _isAttacking;

    [SerializeField] private float _arrivalThreshold;
    [SerializeField] private float _deathDuration;

    [Header("Jump")]
    [Space]
    [SerializeField] private float _jumpForce;
    private const float JumpArcHeight = 1.5f;
    private Coroutine _jumpCoroutine;
    private Vector3 _jumpStartPosition;
    private Vector3 _jumpEndPosition;

    private void Awake()
    {
        _startPosition = transform.position;
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }
    }

    private void Start()
    {
        _agent.speed = _moveSpeed;
        _agent.stoppingDistance = _attackDistance;
    }

    private void Update()
    {
        if (!GameManager.Instance.CanPlay()) return;
        
        switch (_state)
        {
            case EZombieState.Idle:
                Idle();
                break;

            case EZombieState.Trace:
                Trace();
                break;

            case EZombieState.Jump:
                Jump();
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
        }
    }

    private void EnterIdleState()
    {
        _state = EZombieState.Idle;
        _enterIdleTime = Time.time;
    }

    private void Idle()
    {
        if (transform.IsInRange(_player.transform.position, _detectDistance))
        {
            _state = EZombieState.Trace;
            _agent.speed = _runSpeed;
            _animator.SetTrigger("IdleToTrace");
            return;
        }
        
        if (Time.time - _enterIdleTime >= _waitPatrolTime)
        {
            _state = EZombieState.Patrol;
            _agent.speed = _moveSpeed;
            _animator.SetTrigger("IdleToPatrol");
        }
    }

    private void Move(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);
    }

    private void Jump()
    {
        if (_jumpCoroutine != null) return;
        _jumpCoroutine = StartCoroutine(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        Vector3 direction = (_jumpEndPosition - _jumpStartPosition);
        direction.y = 0.0f;
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(direction);

        float jumpHeight = _jumpEndPosition.y - _jumpStartPosition.y + JumpArcHeight;
        float jumpDuration = jumpHeight / _jumpForce;
        float elapsedTime = 0f;

        while (elapsedTime < jumpDuration)
        {
            float progress = elapsedTime / jumpDuration;

            Vector3 currentPosition = Vector3.Lerp(_jumpStartPosition, _jumpEndPosition, progress);
            currentPosition.y += jumpHeight * Mathf.Sin(progress * Mathf.PI);
            _agent.Warp(currentPosition);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _agent.Warp(_jumpEndPosition);
        _agent.CompleteOffMeshLink();
        _agent.isStopped = false;
        _state = EZombieState.Trace;
        _animator.SetTrigger("JumpToTrace");
        _jumpCoroutine = null;
    }

    private void Trace()
    {
        Move(_player.transform.position);
        float distance = transform.GetSquaredDistance(_player.transform.position);
        
        if (Util.IsInRange(distance, _attackDistance))
        {
            _state = EZombieState.Attack;
            _agent.ResetPath();
            _animator.SetTrigger("TraceToAttack");
        }
        else if (!Util.IsInRange(distance, _detectDistance))
        {
            _state = EZombieState.Comeback;
            _agent.ResetPath();
            _agent.speed = _moveSpeed;
            _animator.SetTrigger("TraceToComeback");
            return;
        }

        if (_agent.isOnOffMeshLink)
        {
            OffMeshLinkData linkData = _agent.currentOffMeshLinkData;
            _jumpStartPosition = linkData.startPos;
            _jumpEndPosition = linkData.endPos;
            if (_jumpEndPosition.y > _jumpStartPosition.y)
            {
                _state = EZombieState.Jump;
                _animator.SetTrigger("TraceToJump");
                _agent.isStopped = true;
                _agent.ResetPath();
            }
        }
    }

    private void Comeback()
    {
        Move(_startPosition);
        
        if (transform.IsInRange(_player.transform.position, _detectDistance))
        {
            _agent.ResetPath();
            _agent.speed = _runSpeed;
            _animator.SetTrigger("ComebackToTrace");
            _state = EZombieState.Trace;
            return;
        }

        if (transform.IsInRange(_startPosition, _arrivalThreshold))
        {
            EnterIdleState();
            _agent.ResetPath();
            _animator.SetTrigger("ComebackToIdle");
        }
    }

    private void OnStartPatrol()
    {
        _agent.ResetPath();
        _agent.speed = _moveSpeed;
        _patrolDestination = GetRandomPositionInRange(_startPosition, _patrolDistance);
        _state = EZombieState.Patrol;
    }

    private void Patrol()
    {
        Move(_patrolDestination);
        
        if (transform.IsInRange(_player.transform.position, _detectDistance))
        {
            _state = EZombieState.Trace;
            _agent.ResetPath();
            _agent.speed = _runSpeed;
            _animator.SetTrigger("PatrolToTrace");
            return;
        }
        
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            OnStartPatrol();
        }
    }

    private Vector3 GetRandomPositionInRange(Vector3 center, float range)
    {
        Vector2 randomPosition = Random.insideUnitCircle * range;
        Vector3 nextPosition = new Vector3(randomPosition.x, 0f, randomPosition.y);
        nextPosition += center;
        return nextPosition;
    }

    private void Attack()
    {
        if (!transform.IsInRange(_player.transform.position, _attackDistance))
        {
            _attackTimer = 0f;
            _state = EZombieState.Trace;
            _agent.speed = _runSpeed;
            _animator.SetTrigger("AttackToTrace");
            return;
        }

        if (_isAttacking) return;
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackInterval)
        {
            _isAttacking = true;
            _animator.SetTrigger("Attack");
        }
    }

    public void EndAttack()
    {
        _isAttacking = false;
        _attackTimer = 0;
    }
    
    private void ApplyKnockback(Damage damage)
    {
        Vector3 direction = (transform.position - damage.HitPosition);
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
            _agent.Move(direction * (knockbackForce * _knockbackRate * Time.deltaTime));
            yield return null;
        }
    }

    public bool TryTakeDamage(in Damage damage)
    {
        if (_state == EZombieState.Death || _state == EZombieState.Hit) return false;
        
        _health.Consume(damage.Value);
        _agent.isStopped = true;
        _agent.ResetPath();
        
        if (_health.Value <= 0)
        {
            _state = EZombieState.Death;
            _animator.SetTrigger("Death");
        }
        else
        {
            _state = EZombieState.Hit;
            _animator.SetTrigger("Hit");
            ApplyKnockback(damage);
        }
        return true;
    }

    public void Death()
    {
        Destroy(gameObject);
    }

    public void EndHit()
    {
        StopCoroutine(_knockbackCoroutine);
        EnterIdleState();
    }
}
