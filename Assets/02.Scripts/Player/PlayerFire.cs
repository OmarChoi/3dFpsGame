using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private float _throwPower = 15f;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Bomb bomb = Instantiate(_bombPrefab, _fireTransform.position, Quaternion.identity);
            Rigidbody rigidbody = bomb.GetComponent<Rigidbody>();
            rigidbody.AddForce(Camera.main.transform.forward * _throwPower, ForceMode.Impulse);
        }
    }
}
