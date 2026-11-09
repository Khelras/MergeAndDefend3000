using UnityEngine;

public class LazerTower : Tower
{
    [Header("Laser")]
    [SerializeField] private LineRenderer m_lineRenderer;
    [SerializeField] private Transform m_laserOrigin; 
    [SerializeField] private AudioSource m_laserAudioSource; 

    void Awake()
    {
        m_lineRenderer.positionCount = 2;
        m_lineRenderer.enabled = false;

        m_laserAudioSource.clip = SFXManager.Instance.LazerSFX;
        m_laserAudioSource.loop = true;
        m_laserAudioSource.spatialBlend = 0f;
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
        if (hasTarget)
        {
            if (!m_laserAudioSource.isPlaying) m_laserAudioSource.Play();
            m_lineRenderer.SetPosition(0, m_laserOrigin.position);
            m_lineRenderer.SetPosition(1, m_currentEnemy.transform.position);
        }
        else if (m_laserAudioSource.isPlaying)
        {
            m_laserAudioSource.Stop();
        }
    }
}
