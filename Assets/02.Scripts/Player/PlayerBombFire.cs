using UnityEngine;

public class PlayerBombFire : MonoBehaviour
{
    [SerializeField] private Transform _fireTransform;
    [SerializeField] private float _throwPower = 15f;
    [SerializeField] private int _bombCount = 5;
    public int BombCount => _bombCount;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && _bombCount > 0)
        {
            _bombCount--;
            Bomb bomb = BombFactory.Instance.Get(_fireTransform.position);
            bomb.Launch(Camera.main.transform.forward, _throwPower);
        }
    }
}
