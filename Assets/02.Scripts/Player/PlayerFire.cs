using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _throwPower = 15f;
    private int _bombCount = 5;
    public int BombCount => _bombCount;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _bombCount > 0)
        {
            _bombCount--;
            Bomb bomb = BombFactory.Instance.Get(_fireTransform.position);
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody?.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);
        }
    }
}
