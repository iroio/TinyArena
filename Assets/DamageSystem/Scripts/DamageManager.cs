using UnityEngine;

public class DamageManager : MonoBehaviour
{
    [SerializeField] ProjectileManager _projectileManager;

    public int GetDamage()
    {
        if (_projectileManager == null) return 0;

        BulletData data = _projectileManager.CurrentBulletData;

        if(data == null) return 0;

        return data.damage;
    }
}
