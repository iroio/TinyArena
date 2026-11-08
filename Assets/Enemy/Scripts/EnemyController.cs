using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private bool _isInPool;

    internal void InitEnemy(EnemyManager enemyManager)
    {
        if (enemyManager == null) return;

        _enemyManager = enemyManager;
        _isInPool = true;
    }

    internal void EnemyActivateAt(Vector3 position)
    {
        if (_enemyManager == null) return;

        transform.position = position;
        _isInPool = false;
        gameObject.SetActive(true);
    }

    public void EnemyReturnToPool()
    {
        if (_isInPool) return;
        if (_enemyManager == null) return;
        if (_enemyManager.EnemyPool == null) return;

        _isInPool = true;
        gameObject.SetActive(false);
        _enemyManager.EnemyPool.Set(this);
    }
}
