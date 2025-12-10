using UnityEngine;
using UnityEngine.Pool;

public class BombFactory : MonoBehaviour
{
    private static BombFactory _instance;
    public static BombFactory Instance => _instance;
    
    private IObjectPool<Bomb> _bombPool;
    private int _defaultCapacity = 5;
    [SerializeField] private Bomb _bombPrefab;
    [SerializeField] private ParticleSystem _bombEffect;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        _bombPool = new ObjectPool<Bomb>(CreateBomb, OnGetBomb, OnReleaseBomb, OnDestroyBomb, true, _defaultCapacity);
    }
    
    private Bomb CreateBomb()
    {
        Bomb bomb = Instantiate(_bombPrefab, this.transform);
        bomb.SetEffect(_bombEffect);
        bomb.gameObject.SetActive(false);
        return bomb;
    }

    private void OnGetBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(true);
    }

    private void OnReleaseBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(false);
    }

    private void OnDestroyBomb(Bomb bomb)
    {
        Destroy(bomb.gameObject);
    }
    
    public Bomb Get(Vector3 position)
    {
        Bomb bomb = _bombPool.Get();
        bomb.transform.position = position;
        return bomb;
    }

    public void Release(Bomb bomb)
    {
        _bombPool.Release(bomb);
    }
}
