using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ZombieMovement))]
public class Zombie : MonoBehaviour, IDamageable
{
    // FSM
    private EZombieState _currentStateType = EZombieState.Idle;
    private Dictionary<EZombieState, ZombieStateBase> _states;
    private ZombieStateBase _currentState;

    // Stats (Inspector)
    [Header("Stats")]
    [SerializeField] private ZombieStats _stats;
    [SerializeField] private ConsumableStat _health;

    // References (Inspector)
    [Header("References")]
    [SerializeField] private GameObject _player;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    [SerializeField] private ZombieMovement _movement;

    [SerializeField] private GameObject _bloodEffectPrefab;

    // Runtime
    private Vector3 _startPosition;
    private Damage _lastDamage;
    private JumpData _pendingJumpData;

    // Properties
    public ZombieStateBase CurrentState => _currentState;

    public ZombieStats Stats => _stats;
    public ConsumableStat Health => _health;

    public NavMeshAgent Agent => _agent;
    public Animator Animator => _animator;
    public ZombieMovement Movement => _movement;

    public Transform Player => _player.transform;
    public Vector3 StartPosition => _startPosition;
    public Damage LastDamage => _lastDamage;
    public JumpData PendingJumpData => _pendingJumpData;

    private void Awake()
    {
        _startPosition = transform.position;
        if (_animator == null)
        {
            _animator = GetComponentInChildren<Animator>();
        }

        _movement = gameObject.GetComponent<ZombieMovement>();
        _movement.Init(_agent, _stats);

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        _states = new Dictionary<EZombieState, ZombieStateBase>
        {
            { EZombieState.Idle, new ZombieIdleState(this) },
            { EZombieState.Trace, new ZombieTraceState(this) },
            { EZombieState.Comeback, new ZombieComebackState(this) },
            { EZombieState.Patrol, new ZombiePatrolState(this) },
            { EZombieState.Jump, new ZombieJumpState(this) },
            { EZombieState.Attack, new ZombieAttackState(this) },
            { EZombieState.Hit, new ZombieHitState(this) },
            { EZombieState.Death, new ZombieDeathState(this) }
        };

        _currentState = _states[EZombieState.Idle];
        _currentState.Enter();
    }

    private void Start()
    {
        _health.SetMaxValue(_stats.MaxHealth);
        _health.Initialize();
        _agent.speed = _stats.MoveSpeed;
        _agent.stoppingDistance = _stats.AttackDistance;
    }

    private void Update()
    {
        if (!GameManager.Instance.CanPlay()) return;

        _currentState?.Update();
    }

    public void StopAgent()
    {
        _agent.isStopped = true;
        _agent.ResetPath();
    }
    
    public bool TryTakeDamage(in Damage damage)
    {
        if (_currentStateType == EZombieState.Death || _currentStateType == EZombieState.Hit) return false;

        _health.Consume(damage.Value);
        GameObject bloodEffect = Instantiate(_bloodEffectPrefab, damage.HitPosition, Quaternion.identity, transform);
        bloodEffect.transform.forward = damage.Normal;

        _lastDamage = damage;

        if (_health.Value <= 0)
        {
            TransitionTo(EZombieState.Death);
            _animator.SetTrigger(ZombieAnimatorHash.Death);
        }
        else
        {
            TransitionTo(EZombieState.Hit);
            _animator.SetTrigger(ZombieAnimatorHash.Hit);
        }
        return true;
    }

    public void TransitionTo(EZombieState newState)
    {
        if (_currentStateType == newState) return;

        _currentState?.Exit();
        _currentStateType = newState;
        _currentState = _states[newState];
        _currentState.Enter();
    }

    public void SetPendingJumpData(JumpData jumpData)
    {
        _pendingJumpData = jumpData;
    }
}