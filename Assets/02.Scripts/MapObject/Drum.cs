using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Drum : MonoBehaviour, IDamageable
{
    [SerializeField] private ValueStat _health;
    [SerializeField] private ParticleSystem _explodeEffect;
    private bool _isExplode;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public bool TryTakeDamage(Damage damage)
    {
        if (_isExplode) return false;
        _health.Decrease(damage.Value);
        if (_health.Value <= 0)
        {
            Explode();   
        }
        return true;
    }

    private void Explode()
    {
        _isExplode = true;
        PlayExplodeEffect();
    }

    private void PlayExplodeEffect()
    {
        _explodeEffect.transform.position = transform.position;
        _explodeEffect.Play();
    }
}
