using UnityEngine;

public sealed class EnemyController : MonoBehaviour
{
    private DamageManager _damageManager;
    private EnemyManager _enemyManager;
    private GameObjectPool<EnemyController> _enemyPool;

    int _hash_Player;
    private int _currentHealth;
    private bool _isInPool;

    internal void InitEnemy(EnemyManager enemyManager, GameObjectPool<EnemyController> enemyPool = null)
    {
        if (enemyManager == null) return;

        _enemyManager = enemyManager;
        _damageManager = enemyManager.DamageManager;

        if (enemyPool != null)
            _enemyPool = enemyPool;

        _isInPool = true;
    }

    public void CheckHitPlayer()
    {
        Vector3 rayOrigin = transform.position + Vector3.up;
        Vector3 dir = Vector3.forward;
        bool hitPlayer = Physics.Raycast(rayOrigin, dir, out RaycastHit hit, 3f, _hash_Player);
        Debug.DrawRay(rayOrigin, dir, hitPlayer ? Color.green : Color.red);

        if (hitPlayer)
        {
            PlayerController player = hit.collider.GetComponent<PlayerController>();

            if (player != null)
            {
                player.HitEnemy(this);
            }
        }
    }

    public void HitBullet(ProjectileController projectile)
    {
        if(projectile == null) return;

        _currentHealth -= _damageManager.GetDamage();

        if (_currentHealth <= 0)
        {
            EnemyReturnToPool();
        }
    }

    internal void EnemyActivateAt(Vector3 position)
    {
        if (_enemyManager == null) return;

        transform.position = position;
        _isInPool = false;
        _currentHealth = _enemyManager.CurrentHealth;
        gameObject.SetActive(true);
    }

    public void EnemyReturnToPool()
    {
        if (_isInPool) return;
        if (_enemyManager == null) return;
        if (_enemyPool == null) return;

        _isInPool = true;
        _enemyManager.NotifyEnemyReturned(this);
        gameObject.SetActive(false);
        _enemyPool.Set(this);
    }

    void Awake()
    {
        _hash_Player = 1 << LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        CheckHitPlayer();
    }
}
