using UnityEngine;

public enum DebugEnemyType
{
    Basic,
    Fast
}

// 개발자 패널에서 요청한 적을 Spawn Point 주변에 분산 생성한다.
public sealed class DebugEnemySpawner : MonoBehaviour
{
    [SerializeField] private EnemyManager _enemyManager;
    [SerializeField] private Transform[] _spawnPoints = new Transform[4];
    [SerializeField, Min(0f)] private float _spreadRadius = 3f;
    [SerializeField, Min(0f)] private float _minEnemyDistance = 1f;
    [SerializeField, Min(1)] private int _maxAttemptsPerEnemy = 30;
    [SerializeField] private LayerMask _wallMask;
    [SerializeField, Min(0.01f)] private float _wallCheckRadius = 0.5f;
    [SerializeField] private float _wallCheckHeight = 1f;

    public int SpawnPointCount => _spawnPoints.Length;

    public bool IsEnemyTypeImplemented(DebugEnemyType type)
    {
        return type == DebugEnemyType.Basic;
    }

    // 실제로 생성된 적의 수를 반환한다.
    public int SpawnAround(DebugEnemyType type, int spawnPointIndex, int count)
    {
        if (_enemyManager == null) return 0;
        if (!IsEnemyTypeImplemented(type)) return 0;
        if (spawnPointIndex < 0 || spawnPointIndex >= _spawnPoints.Length) return 0;
        if (_spawnPoints[spawnPointIndex] == null || count < 1) return 0;

        Vector3 center = _spawnPoints[spawnPointIndex].position;
        int spawned = 0;

        for (int i = 0; i < count; i++)
        {
            if (!TryFindSpawnPosition(center, out Vector3 position)) break;

            if (_enemyManager.SpawnEnemy(position) == null) break;
            spawned++;
        }

        return spawned;
    }

    private bool TryFindSpawnPosition(Vector3 center, out Vector3 position)
    {
        for (int attempt = 0; attempt < _maxAttemptsPerEnemy; attempt++)
        {
            Vector2 offset = Random.insideUnitCircle * _spreadRadius;
            Vector3 candidate = new Vector3(center.x + offset.x, center.y, center.z + offset.y);

            if (!_enemyManager.TryGetArenaPosition(candidate, out position)) continue;
            if (IsBlockedByWall(position)) continue;
            if (IsTooCloseToEnemy(position)) continue;

            return true;
        }

        position = Vector3.zero;
        return false;
    }

    private bool IsBlockedByWall(Vector3 position)
    {
        Vector3 checkPoint = position + Vector3.up * _wallCheckHeight;
        return Physics.CheckSphere(checkPoint, _wallCheckRadius, _wallMask, QueryTriggerInteraction.Ignore);
    }

    private bool IsTooCloseToEnemy(Vector3 position)
    {
        float sqrMin = _minEnemyDistance * _minEnemyDistance;
        var enemies = _enemyManager.ActiveEnemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            Vector3 delta = enemies[i].transform.position - position;
            delta.y = 0f;
            if (delta.sqrMagnitude < sqrMin) return true;
        }

        return false;
    }
}
