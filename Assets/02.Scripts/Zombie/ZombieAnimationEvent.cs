using UnityEngine;

public class ZombieAnimationEvent : MonoBehaviour
{
    [SerializeField] private Zombie _zombie;
    [SerializeField] private GameObject _player;
    [SerializeField] private float _damage;
    
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
            Damage damage = new Damage(_damage, _zombie.gameObject);
            damageable.TryTakeDamage(damage);
        }
    }

    public void AttackEnd()
    {
        _zombie.EndAttack();
    }

    public void Death()
    {
        _zombie.Death();
    }

    public void EndHit()
    {
        _zombie.EndHit();
    }
}
