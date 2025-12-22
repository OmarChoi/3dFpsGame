using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Coin : MonoBehaviour
{
    private ECoinType _type;
    private Rigidbody _rigidbody;
    [Header("Scatter Option")]
    [SerializeField] private float _scatterPower;
    [SerializeField] private float _spinPower;
    [SerializeField] private float _scatterAmount;

    [Space]
    [Header("attraction Option")]
    [SerializeField] private float _attractionDuration;
    [SerializeField] private float _controlPointHeight;
    [SerializeField] private float _attractionRotationSpeed;
    private bool _isAttracted;
    private Vector3 _rotationAxis;

    // 베지어 곡선
    private float _attractProgress;
    private Vector3 _startPoint;
    private Vector3 _controlPoint;
    private Transform _targetTransform;
    
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (_isAttracted)
        {
            ProcessAttraction();
        }
    }

    public void Reset()
    {
        _rigidbody.isKinematic = false;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _isAttracted = false;
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
        if (_isAttracted) return;

        _startPoint = transform.position;
        _targetTransform = other.transform;
        Vector3 playerPosition = _targetTransform.position;

        _controlPoint = Vector3.Lerp(_startPoint, playerPosition, 0.5f) + Vector3.up * _controlPointHeight;
        _rotationAxis = Random.insideUnitSphere.normalized;

        _attractProgress = 0f;
        _isAttracted = true;
        _rigidbody.isKinematic = true;

        DOVirtual.Float(0f, 1f, _attractionDuration, value => _attractProgress = value)
                 .SetEase(Ease.InQuad)
                 .OnComplete(ReleaseCoin);
    }

    private void ProcessAttraction()
    {
        Vector3 currentPlayerPosition = _targetTransform.position;
        Vector3 position = CalculateQuadraticBezier(
            _attractProgress,
            _startPoint, 
            _controlPoint,
            currentPlayerPosition);
        
        transform.position = position;

        transform.Rotate(_rotationAxis, _attractionRotationSpeed * Time.deltaTime);
    }

    private Vector3 CalculateQuadraticBezier(float normalizedTime, Vector3 startPosition, Vector3 controlPoint, Vector3 endPosition)
    {
        float oneMinusT = 1f - normalizedTime;
        float tSquared = normalizedTime * normalizedTime;
        float oneMinusTSquared = oneMinusT * oneMinusT;

        Vector3 position = oneMinusTSquared * startPosition +
                           2f * oneMinusT * normalizedTime * controlPoint +
                           tSquared * endPosition;
        return position;
    }

    private void ReleaseCoin()
    {
        CoinFactory.Instance.ReleaseCoin(_type, this);
    }
}
