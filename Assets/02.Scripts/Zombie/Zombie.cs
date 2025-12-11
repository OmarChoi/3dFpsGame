using UnityEngine;

public class Zombie : MonoBehaviour
{
    public EZombieState State = EZombieState.Idle;

    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private CharacterController _characterController;
    
    [Header("Move")]
    [Space]
    [SerializeField] private float _detectDistance = 4f;
    [SerializeField] private float _moveSpeed = 5.0f;
    
    [Header("Attack")]
    [Space]
    [SerializeField] private float _attackDistance = 1.5f;
    [SerializeField] private float _damage = 20.0f;
    [SerializeField] private float _attackSpeed = 2.0f;
    private float _attackTimer = 2.0f;
    
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
            
            case EZombieState.Hit:
                Hit();
                break;
            
            case EZombieState.Death:
                Death();
                break;
        }
    }

    private void Idle()
    {
        // Todo. Idle Animation 실행
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectDistance)
        {
            State = EZombieState.Trace;
        }
    }

    private void Trace()
    {
        // Todo. Run Animation 실행
        Vector3 direction = (_player.transform.position - transform.position).normalized;
        _characterController.Move(direction * (_moveSpeed * Time.deltaTime));
        
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance <= _attackDistance)
        {
            State = EZombieState.Attack;
        }
    }

    private void Comeback()
    {
        // Todo. Run Animation 실행
        
    }

    private void Attack()
    {
        float distance = Vector3.Distance(transform.position, _player.transform.position);
        if (distance > _attackDistance)
        {
            State = EZombieState.Trace;
            return;
        }

        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _attackSpeed)
        {
            _attackTimer = 0f;
            _playerStats?.Health.Decrease(_damage);
        }
    }

    private void Hit()
    {

    }

    private void Death()
    {
        
    }
}
