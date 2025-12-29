using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawner : MonoBehaviour
{
    // Referece
    [SerializeField] private GameObject _zombiePrefab;
    [SerializeField] private GameObject _player;
    [SerializeField] private Terrain _terrain;
    
    // Spawn Offset
    [SerializeField] private float _spawnMinRatio = 0.1f;   // Spawn되지 않는 영역 비율
    [SerializeField] private int _maxZombieCount;
    
    // Spawn Valid Check (NavMesh)
    [SerializeField] private float _navMeshSampleDistance;  // 근처에 NavMesh 존재 여부 확인 거리
    [SerializeField] private int _maxSpawnAttempts;
    
    private void Awake()
    {
        SpawnZombies();
    }

    private void SpawnZombies()
    {
        if (_terrain == null || _zombiePrefab == null)
        {
            Debug.LogError("Terrain or Zombie Prefab is not assigned!");
            return;
        }

        Vector3 terrainSize = _terrain.terrainData.size;
        Vector3 terrainPosition = _terrain.transform.position;

        for (int i = 0; i < _maxZombieCount; i++)
        {
            Vector3 spawnPosition = GetValidSpawnPosition(terrainSize, terrainPosition);
            if (spawnPosition == Vector3.zero) continue;
            GameObject zombie = Instantiate(_zombiePrefab, spawnPosition, Quaternion.identity, transform);
            zombie.GetComponent<Zombie>().SetPlayer(_player);
        }
    }

    private Vector3 GetValidSpawnPosition(Vector3 terrainSize, Vector3 terrainPosition)
    {
        for (int attempt = 0; attempt < _maxSpawnAttempts; attempt++)
        {
            float randomX = Random.Range(terrainSize.x * _spawnMinRatio, terrainSize.x * (1 - _spawnMinRatio));
            float randomZ = Random.Range(terrainSize.z * _spawnMinRatio, terrainSize.z * (1 - _spawnMinRatio));

            float height = _terrain.SampleHeight(new Vector3(terrainPosition.x + randomX, 0, terrainPosition.z + randomZ));

            Vector3 candidatePosition = new Vector3(
                terrainPosition.x + randomX,
                terrainPosition.y + height,
                terrainPosition.z + randomZ
            );

            if (NavMesh.SamplePosition(candidatePosition, out NavMeshHit hit, _navMeshSampleDistance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return Vector3.zero;
    }
}
