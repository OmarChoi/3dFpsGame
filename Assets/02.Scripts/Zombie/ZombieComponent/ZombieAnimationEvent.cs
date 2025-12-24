using UnityEngine;

public class ZombieAnimationEvent : MonoBehaviour
{
    [SerializeField] private Zombie _zombie;
    [SerializeField] private GameObject _player;
    
    private void Awake()
    {
        if (_zombie == null)
        {
            _zombie = GetComponentInParent<Zombie>();
        }
        _player = GameObject.FindWithTag("Player");
    }

    public void AttackPlayer()
    {
        if (_player.TryGetComponent(out IDamageable damageable))
        {
            Damage damage = new Damage()
            {
                Value = _zombie.Stats.Damage, 
                HitPosition = _player.transform.position,
                Normal = (_player.transform.position - _zombie.transform.position).normalized,
                Attacker = _zombie.gameObject,
                Critical = false,
            };
            damageable.TryTakeDamage(damage);
        }
    }

    public void OnAnimationEnd()
    {
        (_zombie.CurrentState as IAnimationEventHandler)?.OnAnimationEnd();
    }
}
