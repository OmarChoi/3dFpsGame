using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private ExplosionData _explosionData;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        _explosionData.IsExploded = false;
        _rigidbody.linearVelocity = Vector3.zero;
    }
    
    public void SetEffect(ParticleSystem effect)
    {
        _explosionData.Effect = effect;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_explosionData.IsExploded) return;
        _explosionData.IsExploded = true;
        _explosionData.Effect.transform.position = transform.position;
        _explosionData.Effect.Play();
        ApplyBombDamage();
        BombFactory.Instance.Release(this);
    }

    private void ApplyBombDamage()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionData.Radius, _explosionData.LayerMask);
        Damage damage = new Damage(_explosionData.Damage, gameObject);
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
