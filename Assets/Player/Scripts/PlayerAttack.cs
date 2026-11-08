using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform _firePos;
    [SerializeField] float _fireRate = 0.5f;
    [SerializeField] float _projectileSpeed = 12f;

    ArenaInputReader _arenaInput;
    ProjectileManager _projectileManager;

    Coroutine _fireCoroutine;

    IEnumerator CoFireProjectile()
    {
        while (_arenaInput.IsFirePressed)
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

    public ProjectileController FireProjectile(Vector3 requestedPosition)
    {
        var projectile = _projectileManager.ProjectilePool.Get();

        if(projectile == null) return null;

        projectile.ProjActiveAt(requestedPosition);

        projectile.Launch(_firePos.forward, _projectileSpeed);

        return projectile;
    }

    void Start()
    {
        _arenaInput = GetComponent<ArenaInputReader>();
        _projectileManager = FindFirstObjectByType<ProjectileManager>();
    }

    void Update()
    {
        if (_arenaInput == null) return;

        if (_arenaInput.IsFirePressed)
        {
            StartCoFireProjectile();
        }
    }
}
