using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private GameObject _explosionEffectPrefab;
    
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(_explosionEffectPrefab);
        effectObject.transform.position = transform.position;
        
        BombFactory.Instance.Release(this);
    }
}
