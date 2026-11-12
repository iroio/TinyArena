using UnityEngine;
public enum BulletType
{
    Normal,
    Explosive,
}

[CreateAssetMenu(menuName = "Bullet/Bullet Data")]
public class BulletData : ScriptableObject
{
    public BulletType bulletType;

    public GameObject prefab;

    [Range(0,5)]
    public int typeNum;

    [Range(0, 100)]
    public int speed;

    public float fireRate;

    public int penetration;
    public int range;

    public int damage;
}