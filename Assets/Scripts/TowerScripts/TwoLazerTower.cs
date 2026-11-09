using UnityEngine;

public class TwoLazerTower : Tower
{
    [Header("Laser")]
    [SerializeField] private LineRenderer[] m_lineRenderers; // size 2
    [SerializeField] private Transform[] m_laserOrigins;     // size 2

    private EnemyHealth[] m_targets;

    void Awake()
    {
        m_targets = new EnemyHealth[m_lineRenderers.Length];
        foreach (LineRenderer lr in m_lineRenderers)
        {
            lr.positionCount = 2;
            lr.enabled = false;
        }
    }

    public override void Attack()
    {
        AssignTargets();

        foreach (EnemyHealth target in m_targets)
        {
            if (target != null)
                target.TakeDamage(m_projectileDamaged);
        }
    }

    private void AssignTargets()
    {
        
        m_targets[0] = m_currentEnemy;

        
        for (int i = 1; i < m_targets.Length; i++)
        {
            m_targets[i] = m_currentEnemy; 

            foreach (EnemyHealth enemy in m_currentEnemiesInRange)
            {
                if (enemy != null && System.Array.IndexOf(m_targets, enemy, 0, i) < 0)
                {
                    m_targets[i] = enemy;
                    break;
                }
            }
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < m_lineRenderers.Length; i++)
        {
            EnemyHealth target = m_targets[i];
            bool hasTarget = target != null && m_currentEnemiesInRange.Contains(target);

            m_lineRenderers[i].enabled = hasTarget;
            if (!hasTarget) continue;

            m_lineRenderers[i].SetPosition(0, m_laserOrigins[i].position);
            m_lineRenderers[i].SetPosition(1, target.transform.position);
        }
    }
}

