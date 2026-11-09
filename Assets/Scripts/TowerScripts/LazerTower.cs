using UnityEngine;

public class LazerTower : Tower
{
    [Header("Laser")]
    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private Transform m_laserOrigin; // empty child at the tip of the head

    void Awake()
    {
        m_lineRenderer.positionCount = 2;
        m_lineRenderer.enabled = false;
    }

    public override void Attack()
    {
        m_currentEnemy.TakeDamage(m_projectileDamaged);
    }

   
    void LateUpdate()
    {
        bool hasTarget = m_currentEnemy != null &&
                         m_currentEnemiesInRange.Contains(m_currentEnemy);

        m_lineRenderer.enabled = hasTarget;
        if (!hasTarget) return;

        m_lineRenderer.SetPosition(0, m_laserOrigin.position);
        m_lineRenderer.SetPosition(1, m_currentEnemy.transform.position);
    }
}
