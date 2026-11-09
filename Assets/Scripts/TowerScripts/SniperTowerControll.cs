using UnityEngine;

public class SniperTowerControll : Tower
{
    GameObject bullet;
    [SerializeField] private Transform m_projectileSpawnPos;
    public override void Attack()
    {
        bullet = Instantiate(m_projectilePrefab, m_projectileSpawnPos.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().Initialize(m_currentEnemy, m_projectileDamaged, m_projectileSpeed);
        SFXManager.Instance.PlaySFX(SFXManager.Instance.SniperSFX, 1f);
    }
}
