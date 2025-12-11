using System.Collections;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public EZombieState State = EZombieState.Idle;

    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private CharacterController _characterController;
    
    private float _health = 100.0f;
    private Vector3 _startPosition;
    
    private Coroutine _knockbackCoroutine;
    private float _hitDuration = 0.3f;
    
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

    private void Awake()
    {
        _startPosition = transform.position;
        _attackTimer = _attackSpeed;
    }
    
    private void Update()
    {
        switch (State)
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
        }
    }

    private void Idle()
    {
        // Todo. Idle Animation 실행
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectDistance)
        {
            Debug.Log("Change State Idle -> Trace");
            State = EZombieState.Trace;
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
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= _attackDistance)
        {
            Debug.Log("Change State Trace -> Attack");
            State = EZombieState.Attack;
        }
        else if (distance >= _detectDistance)
        {
            Debug.Log("Change State Trace -> Comeback");
            State = EZombieState.Comeback;
        }
    }

    private void Comeback()
    {
        // Todo. Run Animation 실행
        Move(_startPosition);
        float distance = Vector3.Distance(transform.position, _startPosition);
        if (distance <= 1.0f)
        {
            Debug.Log("Change State Comeback -> Idle");
            State = EZombieState.Idle;
        }
    }

    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > _attackDistance)
        {
            Debug.Log("Change State Attack -> Trace");
            State = EZombieState.Trace;
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            Debug.Log("Attack");
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
        if (State == EZombieState.Death || State == EZombieState.Hit) return false;
        _health -= damage;
        if (_health <= 0)
        {
            State = EZombieState.Death;
            StartCoroutine(DeathCoroutine());
        }
        else
        {
            State = EZombieState.Hit;
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
        Debug.Log("Hit");
        // Todo. Hit Animation 실행
        yield return new WaitForSeconds(_hitDuration);
        State = EZombieState.Idle;
    }

    private IEnumerator DeathCoroutine()
    {
        Debug.Log("Death");
        // Todo. Death Animation 실행
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
