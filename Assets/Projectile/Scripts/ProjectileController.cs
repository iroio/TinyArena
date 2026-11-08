using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    PlayerAttack _playerAttack;

    private ProjectileManager _projectileManager;
    private bool _isInPool;

    Vector3 _prevPos;
    Vector3 _currentPos;

    Rigidbody _rb;

    int _hash_Wall;
    int _hash_Enemy;

    internal void InitProjectile(ProjectileManager projManager)
    {
        if (projManager == null) return;

        _projectileManager = projManager;
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

        transform.position = position;
        _prevPos = position;
        _isInPool = false;
        gameObject.SetActive(true);
    }

    public void CheckHit()
    {
        _currentPos = transform.position;

        Vector3 moveDirection = _currentPos - _prevPos;
        float distance = moveDirection.magnitude;

        if (distance > 0f)
        {
            Vector3 dir = moveDirection.normalized;
            if (Physics.Raycast(_prevPos,dir,out RaycastHit hit, distance, _hash_Wall) ||
                Physics.Raycast(_prevPos, dir, out hit, distance, _hash_Enemy))
            {
                ReturnToPool();
                return;
            }
        }

        _prevPos = _currentPos;
    }

    public void ReturnToPool()
    {
        if (_isInPool) return;
        if (_projectileManager == null) return;
        if (_projectileManager.ProjectilePool == null) return;

        _isInPool = true;

        _rb.linearVelocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;

        gameObject.SetActive(false);
        _projectileManager.ProjectilePool.Set(this);
    }

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();

        _hash_Wall = 1 << LayerMask.NameToLayer("Wall");
        _hash_Enemy = 1 << LayerMask.NameToLayer("Enemy");
    }

    void Update()
    {
        CheckHit();
    }
}
