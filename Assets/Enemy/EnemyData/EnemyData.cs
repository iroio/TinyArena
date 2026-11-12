using UnityEngine;
public enum EnemyType
{
    Normal,
    Fast,
}

[CreateAssetMenu(menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public EnemyType enemyType;

    public GameObject prefab;

    [Range(0,5)]
    public int typeNum;

    // ÃâÇö È®·ü
    [Range(0, 100)]
    public int weight;

    [Range(0, 20)]
    public int speed;

    public int health;
    public int damage;
    public float reach;
}