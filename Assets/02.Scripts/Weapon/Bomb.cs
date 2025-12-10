using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private ParticleSystem _explosionEffect;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void Reset()
    {
        _rigidbody.linearVelocity = Vector3.zero;
    }
    
    public void SetEffect(ParticleSystem effect)
    {
        _explosionEffect = effect;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        _explosionEffect.transform.position = this.transform.position;
        _explosionEffect.Play();
        BombFactory.Instance.Release(this);
    }

    public void Launch(Vector3 direction, float power)
    {
        _rigidbody?.AddForce(direction * power, ForceMode.Impulse);
    }
}
