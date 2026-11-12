using System.Collections.Generic;
using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    private readonly List<EnemyController> _activeEnemies = new List<EnemyController>();

    public IReadOnlyList<EnemyController> ActiveEnemies => _activeEnemies;
    public int ActiveEnemyCount => _activeEnemies.Count;

    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] Transform _enemyRoot;

    [SerializeField] private MeshCollider _arenaFloorCollider;
    [SerializeField, Min(1)] private int _initialPoolSize = 10;
    [SerializeField] private List<EnemyData> _enemyData;
    [SerializeField] private DamageManager _damageManager;

    private readonly Dictionary<EnemyData, GameObjectPool<EnemyController>> _enemyPools =
        new Dictionary<EnemyData, GameObjectPool<EnemyController>>();

    private GameObjectPool<EnemyController> _enemyPool;
    private EnemyData _currentEnemyData;

    public GameObjectPool<EnemyController> EnemyPool => _enemyPool;
    public EnemyData CurrentEnemyData => _currentEnemyData;
    public int CurrentHealth => _currentEnemyData != null ? _currentEnemyData.health : 0;
    public DamageManager DamageManager => _damageManager;

    public EnemyController SpawnEnemy(Vector3 requestedPosition)
    {
        if (_enemyPool == null) return null;
        if (!TryGetArenaPosition(requestedPosition, out Vector3 spawnPosition)) return null;

        EnemyController enemy = EnemyPool.Get();
        if (enemy == null)
        {
            return null;
        }
        enemy.InitEnemy(this, _enemyPool);
        enemy.EnemyActivateAt(spawnPosition);
        _activeEnemies.Add(enemy);
        return enemy;
    }

    public void ChangeData(EnemyData data)
    {
        if (data == null) return;
        if (!_enemyPools.TryGetValue(data, out GameObjectPool<EnemyController> pool))
        {
            Debug.LogWarning($"EnemyData '{data.name}' has no enemy pool. Add it to the manager list.", data);
            return;
        }

        _currentEnemyData = data;
        _enemyPool = pool;
    }

    internal void NotifyEnemyReturned(EnemyController enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    public bool TryGetArenaPosition(Vector3 requestedPosition, out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;
        Bounds floorBounds = _arenaFloorCollider.bounds;
        Vector3 rayOrigin = new Vector3(requestedPosition.x, 
            floorBounds.max.y + 1f, requestedPosition.z);
        float rayDistance = floorBounds.size.y + 2f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        bool hitFloor = _arenaFloorCollider.Raycast(ray, out RaycastHit floorHit, rayDistance);
        Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitFloor ? Color.green : Color.red);

        if (!hitFloor)
        {
            return false;
        }

        spawnPosition = new Vector3(requestedPosition.x, floorHit.point.y, requestedPosition.z);
        return true;
    }

    private void Awake()
    {
        if (_arenaFloorCollider == null) return;

        if (_enemyData != null)
        {
            for (int i = 0; i < _enemyData.Count; i++)
            {
                EnemyData data = _enemyData[i];
                if (data == null || data.prefab == null || _enemyPools.ContainsKey(data)) continue;

                EnemyController prefab = data.prefab.GetComponent<EnemyController>();
                if (prefab == null)
                {
                    Debug.LogError($"EnemyData '{data.name}' prefab requires an EnemyController.", data);
                    continue;
                }

                _enemyPools.Add(data, CreatePool(prefab));
                if (_currentEnemyData == null)
                {
                    _currentEnemyData = data;
                    _enemyPool = _enemyPools[data];
                }
            }
        }

        if (_enemyPool == null && _enemyPrefab != null)
            _enemyPool = CreatePool(_enemyPrefab);
    }

    private GameObjectPool<EnemyController> CreatePool(EnemyController prefab)
    {
        return new GameObjectPool<EnemyController>(_initialPoolSize, () =>
        {
            EnemyController enemy = Instantiate(prefab, _enemyRoot);
            enemy.gameObject.SetActive(false);
            enemy.InitEnemy(this);
            return enemy;
        });
    }
}
