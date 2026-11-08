using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    [SerializeField] private ProjectileController _projectilePrefab;
    [SerializeField] Transform _projectileRoot;
    [SerializeField, Min(1)] private int _initialPoolSize = 10;

    GameObjectPool<ProjectileController> _projectilePool;

    public GameObjectPool<ProjectileController> ProjectilePool => _projectilePool;
    public void ProjReturnToPool(ProjectileController proj)
    {
        if(!proj.gameObject.activeSelf) return;

        proj.ReturnToPool();
    }

    void Awake()
    {
        if (_projectilePrefab == null) return;
        if(_projectileRoot == null) return;

        _projectilePool = new GameObjectPool<ProjectileController>(_initialPoolSize, () =>
        {
            ProjectileController projectile = Instantiate(_projectilePrefab, _projectileRoot);
            projectile.gameObject.SetActive(false);
            projectile.InitProjectile(this);

            return projectile;
        });
    }
}