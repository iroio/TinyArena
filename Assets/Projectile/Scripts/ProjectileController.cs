using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private ProjectileManager _projectileManager;
    private GameObjectPool<ProjectileController> _projectilePool;
    private bool _isInPool;

    Vector3 _prevPos;
    Vector3 _currentPos;

    Rigidbody _rb;

    int _hash_Wall;
    int _hash_Enemy;

    public Vector3 CurrentPos => _currentPos;

    internal void InitProjectile(ProjectileManager projManager, GameObjectPool<ProjectileController> projectilePool = null)
    {
        if (projManager == null) return;

        _projectileManager = projManager;
        if (projectilePool != null)
            _projectilePool = projectilePool;
        _isInPool = true;
    }   

    public void Launch(Vector3 direction, float speed)
    {
        if (_rb != null)
        {
            _rb.linearVelocity = direction.normalized * speed;
        }
    }

    internal void ProjActiveAt(Vector3 position)
    {
        if(_projectileManager == null) return;

        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        transform.position = position;
        _prevPos = position;
        _isInPool = false;
        gameObject.SetActive(true);
    }

    public void CheckHitEnemy()
    {
        _currentPos = transform.position;

        Vector3 moveDirection = _currentPos - _prevPos;
        float distance = moveDirection.magnitude;

        if (distance > 0f)
        {
            Vector3 dir = moveDirection.normalized;
            bool didHit = Physics.Raycast(_prevPos, dir, out RaycastHit hit, distance, _hash_Wall) ||
                Physics.Raycast(_prevPos, dir, out hit, distance, _hash_Enemy);
            Debug.DrawRay(_prevPos, dir * distance, didHit ? Color.green : Color.red);

            if (didHit)
            {
                EnemyController enemy = hit.collider.GetComponent<EnemyController>();

                if(enemy != null)
                {
                    enemy.HitBullet(this);
                }

                ReturnToPool();
                return;
            }
        }

        _prevPos = _currentPos;
    }

    public void ReturnToPool()
    {
        if (_isInPool) return;
        if (_projectilePool == null) return;

        _isInPool = true;

        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
            _rb.angularVelocity = Vector3.zero;
        }

        gameObject.SetActive(false);
        _projectilePool.Set(this);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _hash_Wall = 1 << LayerMask.NameToLayer("Wall");
        _hash_Enemy = 1 << LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        CheckHitEnemy();
    }
}