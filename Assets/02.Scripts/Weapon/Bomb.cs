using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private ParticleSystem _explosionEffect;
    private bool _isExploded = false;
    [SerializeField] private float _damage;
    [SerializeField] private float _explodeRadius;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        _isExploded = false;
        _rigidbody.linearVelocity = Vector3.zero;
    }
    
    public void SetEffect(ParticleSystem effect)
    {
        _explosionEffect = effect;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_isExploded) return;
        _isExploded = true;
        _explosionEffect.transform.position = transform.position;
        _explosionEffect.Play();
        ApplyBombDamage();
        BombFactory.Instance.Release(this);
    }

    private void ApplyBombDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explodeRadius);
        Damage damage = new Damage(_damage, this.transform.gameObject);
        foreach (Collider hit in colliders)
        {
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TryTakeDamage(damage);
            }
        }
    }
    
    public void Launch(Vector3 direction, float power)
    {
        _rigidbody?.AddForce(direction * power, ForceMode.Impulse);
    }
}
