using UnityEngine;

public sealed class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyPrefab;
    [SerializeField] Transform _enemyRoot;

    [SerializeField] private MeshCollider _arenaFloorCollider;
    [SerializeField, Min(1)] private int _initialPoolSize = 10;

    private GameObjectPool<EnemyController> _enemyPool;

    public GameObjectPool<EnemyController> EnemyPool => _enemyPool;

    public EnemyController SpawnEnemy(Vector3 requestedPosition)
    {
        if (_enemyPool == null) return null;
        if (!TryGetArenaPosition(requestedPosition, out Vector3 spawnPosition)) return null;

        EnemyController enemy = EnemyPool.Get();
        if (enemy == null)
        {
            return null;
        }
        enemy.EnemyActivateAt(spawnPosition);
        return enemy;
    }

    private bool TryGetArenaPosition(Vector3 requestedPosition, out Vector3 spawnPosition)
    {
        spawnPosition = Vector3.zero;
        Bounds floorBounds = _arenaFloorCollider.bounds;
        Vector3 rayOrigin = new Vector3(
            requestedPosition.x,
            floorBounds.max.y + 1f,
            requestedPosition.z);
        float rayDistance = floorBounds.size.y + 2f;

        if (!_arenaFloorCollider.Raycast(
                new Ray(rayOrigin, Vector3.down),
                out RaycastHit floorHit,
                rayDistance))
        {
            return false;
        }

        spawnPosition = new Vector3(requestedPosition.x, floorHit.point.y, requestedPosition.z);
        return true;
    }

    private void Awake()
    {
        if (_enemyPrefab == null) return;
        if (_arenaFloorCollider == null) return;

        _enemyPool = new GameObjectPool<EnemyController>(_initialPoolSize, () =>
        {
            EnemyController enemy = Instantiate(_enemyPrefab, _enemyRoot);
            enemy.gameObject.SetActive(false);
            enemy.InitEnemy(this);

            return enemy;
        });
    }
}
