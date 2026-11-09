using UnityEngine;

public class BoomTowerControll : Tower
{
    [SerializeField] private Transform m_projectileSpawnPos;
    [SerializeField] private float m_blastRadius;
    GameObject bullet;
    public override void Attack()
    {
        bullet = Instantiate(m_projectilePrefab, m_projectileSpawnPos.position, Quaternion.identity);
        bullet.GetComponent<BoomProjectile>().Initialize(m_currentEnemy, m_projectileDamaged, m_projectileSpeed, m_blastRadius);
        SFXManager.Instance.PlaySFX(SFXManager.Instance.CannonSFX, 1f);
    }
}
