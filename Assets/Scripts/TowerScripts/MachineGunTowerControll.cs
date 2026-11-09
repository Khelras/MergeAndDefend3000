using UnityEngine;

public class MachineGunTowerControll : Tower
{
    public Transform m_barrelOne;
    public Transform m_barrelTwo;
    private bool m_shootSequence;
    GameObject bullet;
    private void Start()
    {
        m_shootSequence = true;
    }

    public override void Attack() 
    {
        if (m_shootSequence)
        {
            bullet = Instantiate(m_projectilePrefab, m_barrelOne.position, Quaternion.identity);
            bullet.GetComponent<Projectile>().Initialize(m_currentEnemy, m_projectileDamaged, m_projectileSpeed);
            m_shootSequence = !m_shootSequence;
            SFXManager.Instance.PlaySFX(SFXManager.Instance.MachineGunsSFX, 1f);
        }
        else 
        {
            bullet = Instantiate(m_projectilePrefab, m_barrelTwo.position, Quaternion.identity);
            bullet.GetComponent<Projectile>().Initialize(m_currentEnemy, m_projectileDamaged, m_projectileSpeed);
            m_shootSequence = !m_shootSequence;
            SFXManager.Instance.PlaySFX(SFXManager.Instance.MachineGunsSFX, 1f);
        }
           
      
        
    }
}
