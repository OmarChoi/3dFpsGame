using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class CoinFactory : MonoBehaviour
{
    private static CoinFactory _instance;
    public static CoinFactory Instance => _instance;

    [SerializeField] private CoinInfo[] _coinInfos;
    private float _totalWeight;
    private Dictionary<ECoinType, IObjectPool<Coin>> _coinPools;
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;

        CreateCoinPools();
    }
    
    private void CreateCoinPools()
    {
        _coinPools = new Dictionary<ECoinType, IObjectPool<Coin>>();
        foreach (var coinInfo in _coinInfos)
        {
            CoinInfo info = coinInfo;
            IObjectPool<Coin> pool = new ObjectPool<Coin>(
                () => CreateCoin(info),
                OnGetCoin,
                OnReleaseCoin
            );
            _coinPools.Add(info.Type, pool);
            _totalWeight += coinInfo.Weight;
        }
    }
    
    private Coin CreateCoin(CoinInfo coinInfo)
    {
        Coin coin = Instantiate(coinInfo.Prefab, transform).GetComponent<Coin>();
        coin.gameObject.SetActive(false);
        return coin;
    }
    
    private void OnGetCoin(Coin coin)
    {
        coin.gameObject.SetActive(true);
        coin.Reset();
    }

    private void OnReleaseCoin(Coin coin)
    {
        coin.gameObject.SetActive(false);
    }

    public void SpawnCoins(int nCoin, Vector3 position)
    {
        for (int i = 0; i < nCoin; i++)
        {
            ECoinType type = GetRandomType();
            Coin coin = _coinPools[type].Get();
            coin.transform.position = position;
            coin.Init(type);
        }
    }

    private ECoinType GetRandomType()
    {
        float randomValue = Random.Range(0f, _totalWeight);
        float cumulativeWeight = 0f;

        foreach (var coinInfo in _coinInfos)
        {
            cumulativeWeight += coinInfo.Weight;
            if (randomValue <= cumulativeWeight)
            {
                return coinInfo.Type;
            }
        }

        return _coinInfos[0].Type;
    }

    public void ReleaseCoin(ECoinType type, Coin coin)
    {
        _coinPools[type].Release(coin);
    }
}