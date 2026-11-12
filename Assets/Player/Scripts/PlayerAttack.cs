using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _firePos;

    [SerializeField, Min(0.01f)] float _fireRate = 0.5f;
    [SerializeField, Min(0f)] float _projectileSpeed = 12f;

    ArenaInputReader _arenaInput;
    ProjectileManager _projectileManager;

    Coroutine _fireCoroutine;

    int _penetration;
    int _damage;
    int _range;

    // 개발자 패널 등 UI가 열려 있는 동안 발사를 막는다.
    public bool IsFireBlocked { get; set; }

    IEnumerator CoFireProjectile()
    {
        while (_arenaInput.IsFirePressed && !IsFireBlocked)
        {
            FireProjectile(_firePos.position);

            yield return new WaitForSeconds(_fireRate);
        }

        _fireCoroutine = null;
    }

    public void StartCoFireProjectile()
    {
        if (_fireCoroutine != null) return;

        _fireCoroutine = StartCoroutine(CoFireProjectile());
    }

    public void GetProjectileData()
    {
        SetProjectileData();
    }

    public void SetProjectileData()
    {
        if (_projectileManager == null) return;

        BulletData data = _projectileManager.CurrentBulletData;
        if (data == null) return;

        _projectileSpeed = data.speed;
        _fireRate = data.fireRate > 0f ? data.fireRate : 0.5f;
        _penetration = data.penetration;
        _damage = data.damage;
        _range = data.range;
    }

    public ProjectileController FireProjectile(Vector3 requestedPosition)
    {
        if (_projectileManager == null) return null;

        SetProjectileData();
        GameObjectPool<ProjectileController> pool = _projectileManager.ProjectilePool;
        if (pool == null) return null;

        ProjectileController projectile = pool.Get();

        if (projectile == null) return null;

        projectile.InitProjectile(_projectileManager, pool);
        projectile.ProjActiveAt(requestedPosition);

        projectile.Launch(_firePos.forward, _projectileSpeed);

        return projectile;
    }

    void Awake()
    {

    }

    void Start()
    {
        _arenaInput = GetComponent<ArenaInputReader>();
        _projectileManager = FindFirstObjectByType<ProjectileManager>();
        SetProjectileData();
    }

    void Update()
    {
        if (_arenaInput == null) return;
        if (IsFireBlocked) return;

        if (_arenaInput.IsFirePressed)
        {
            StartCoFireProjectile();
        }
    }
}
