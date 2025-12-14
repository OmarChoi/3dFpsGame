using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private ExplosionData _explosionData;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _explosionData.ExplosionObject = gameObject;
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
        _explosionData.Effect.transform.position = transform.position;
        _explosionData.Effect.Play();
        ApplyBombDamage();
        _explosionData.IsExploded = true;
        BombFactory.Instance.Release(this);
    }

    private void ApplyBombDamage()
    {
        _explosionData.Center = transform.position;
        ExplosionHelper.ApplyExplosionDamage(_explosionData);
    }
    
    public void Launch(Vector3 direction, float power)
    {
        _rigidbody?.AddForce(direction * power, ForceMode.Impulse);
    }
}
