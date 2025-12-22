using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    private ECoinType _type;
    private Rigidbody _rigidbody;
    [SerializeField] private float _attractionDuration;
    
    [Header("Scatter Option")]
    [SerializeField] private float _scatterPower;
    [SerializeField] private float _spinPower;
    [SerializeField] private float _scatterAmount;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    public void Reset()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.WakeUp();
    }

    public void Init(ECoinType type)
    {
        _type = type;
        ScatterUpward();
    }
    
    private void ScatterUpward()
    {
        Vector3 direction = Vector3.up + Random.insideUnitSphere * _scatterAmount;
        Vector3 force = direction.normalized * _scatterPower;
        Vector3 torque = Random.insideUnitSphere * _spinPower;
        
        _rigidbody.AddForce(force, ForceMode.Impulse);
        _rigidbody.AddTorque(torque, ForceMode.Impulse);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        transform.DOMove(other.transform.position, _attractionDuration)
                 .OnComplete(ReleaseCoin);
    }

    private void ReleaseCoin()
    {
        CoinFactory.Instance.ReleaseCoin(_type, this);
    }
}
