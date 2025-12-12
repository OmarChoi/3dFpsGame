using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Drum : MonoBehaviour, IDamageable
{
    [SerializeField] private ValueStat _health;
    [SerializeField] private ExplosionData _explosionData;
    [SerializeField] private float _torqueMultiplier;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public bool TryTakeDamage(Damage damage)
    {
        if (_explosionData.IsExploded) return false;
        _health.Decrease(damage.Value);
        if (_health.Value <= 0)
        {
            Explode();   
        }
        return true;
    }

    private void Explode()
    {
        _explosionData.IsExploded = true;
        PlayExplodeEffect();
        FlyAway();
    }

    private void PlayExplodeEffect()
    {
        _explosionData.Effect.transform.position = transform.position;
        _explosionData.Effect.Emit(1);
    }

    private void FlyAway()
    {
        Vector2 randomCircle = UnityEngine.Random.insideUnitCircle;
        Vector3 randomDirection = new Vector3(randomCircle.x, 1f, randomCircle.y).normalized;
        _rigidbody.AddForce(randomDirection * _explosionData.ExplosionForce, ForceMode.Impulse);
        _rigidbody.AddTorque(UnityEngine.Random.insideUnitSphere * (_explosionData.ExplosionForce * _torqueMultiplier), ForceMode.Impulse);
    }
}
