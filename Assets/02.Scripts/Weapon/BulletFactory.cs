using UnityEngine;
using UnityEngine.Pool;

public class BulletFactory : MonoBehaviour
{
    private static BulletFactory _instance;
    public static  BulletFactory Instance => _instance;
    
    private IObjectPool<Bullet> _bulletPool;
    private const int DefaultCapacity = 20;
    [SerializeField] private GameObject _bulletPrefab;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        _bulletPool = new ObjectPool<Bullet>(CreateBullet, OnGetBullet, OnReleaseBullet, OnDestroyBullet, true, DefaultCapacity);
    }
    
    private Bullet CreateBullet()
    {
        Bullet bullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(transform);
        return bullet;
    }

    private void OnGetBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReleaseBullet(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyBullet(Bullet bullet)
    {
        Destroy(bullet.gameObject);
    }
    
    public Bullet Get(Vector3 startPosition, Vector3 endPosition)
    {
        Bullet bullet = _bulletPool.Get();
        bullet.Init(startPosition, endPosition);
        return bullet;
    }

    public void Release(Bullet bullet)
    {
        _bulletPool.Release(bullet);
    }
}