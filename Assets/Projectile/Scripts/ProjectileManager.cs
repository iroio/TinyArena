using UnityEngine;
using System.Collections.Generic;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ProjectileController _projectilePrefab;
    [SerializeField] Transform _projectileRoot;
    [SerializeField, Min(1)] private int _initialPoolSize = 10;

    [SerializeField] List<BulletData> _bulletData;
    BulletData _currentBulletData;

    readonly Dictionary<BulletData, GameObjectPool<ProjectileController>> _projectilePools =
        new Dictionary<BulletData, GameObjectPool<ProjectileController>>();

    GameObjectPool<ProjectileController> _projectilePool;

    public GameObjectPool<ProjectileController> ProjectilePool => _projectilePool;

    public BulletData CurrentBulletData => _currentBulletData;

    public void ProjReturnToPool(ProjectileController proj)
    {
        if(!proj.gameObject.activeSelf) return;

        proj.ReturnToPool();
    }

    public void ChangeData(BulletData data)
    {
        if (data == null) return;
        if (!_projectilePools.TryGetValue(data, out GameObjectPool<ProjectileController> pool))
        {
            Debug.LogWarning($"BulletData '{data.name}' has no projectile pool. Add it to the manager list.", data);
            return;
        }

        _currentBulletData = data;
        _projectilePool = pool;
    }

    void Awake()
    {
        if (_projectileRoot == null) return;

        if (_bulletData != null)
        {
            for (int i = 0; i < _bulletData.Count; i++)
            {
                BulletData data = _bulletData[i];
                if (data == null || data.prefab == null || _projectilePools.ContainsKey(data)) continue;

                ProjectileController prefab = data.prefab.GetComponent<ProjectileController>();
                if (prefab == null)
                {
                    Debug.LogError($"BulletData '{data.name}' prefab requires a ProjectileController.", data);
                    continue;
                }

                _projectilePools.Add(data, CreatePool(prefab));
                if (_currentBulletData == null)
                {
                    _currentBulletData = data;
                    _projectilePool = _projectilePools[data];
                }
            }
        }

        if (_projectilePool == null && _projectilePrefab != null)
            _projectilePool = CreatePool(_projectilePrefab);
    }

    GameObjectPool<ProjectileController> CreatePool(ProjectileController prefab)
    {
        return new GameObjectPool<ProjectileController>(_initialPoolSize, () =>
        {
            ProjectileController projectile = Instantiate(prefab, _projectileRoot);
            projectile.gameObject.SetActive(false);
            projectile.InitProjectile(this);
            return projectile;
        });
    }
}