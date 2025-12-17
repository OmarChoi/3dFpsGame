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
            _zombie = GetComponent<Zombie>();
        }
        _player = GameObject.FindWithTag("Player");
    }

    public void AttackPlayer()
    {
        if (_player.TryGetComponent(out IDamageable damageable))
        {
            Damage damage = new Damage(_damage, transform.gameObject);
            damageable.TryTakeDamage(damage);
        }
    }

    public void Death()
    {
        _zombie.Death();
    }
}
