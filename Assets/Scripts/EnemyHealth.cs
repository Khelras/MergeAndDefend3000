using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    [SerializeField] private float m_maxHealth = 100;
    [SerializeField] private int m_moneyReward = 10;
    private Player m_player;
    private bool m_isDead;

    public float m_currentHealth;
    public float MaxHealth { get { return m_maxHealth; } }

    private void Awake()
    {
        m_currentHealth = m_maxHealth;
    }

    public void SetPlayerReference(Player player)
    {
        m_player = player;
    }

    public float GetHealthPercentage()
    {
        if (m_maxHealth <= 0)
        {
            return 0f;
        }

        return Mathf.Clamp01(m_currentHealth / m_maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0f || m_currentHealth <= 0f)
        {
            return;
        }

        m_currentHealth -= damage;

        Debug.Log($"Enemy took {damage} damage. Current health: {m_currentHealth}");

        if (m_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (m_isDead)
        {
            return; // Prevent multiple death triggers
        }

        m_isDead = true;


        if (m_player != null)
        {
            m_player.AddMoney(m_moneyReward);
        }

        // Add death logic here (e.g., play animation, drop loot, etc.)
        Destroy(gameObject);
        SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyDeathSFX, 1f);
    }

    public void SetHealthMultiplier(float multiplier)
    {
        // Ensure the multiplier is at least 1 to avoid reducing health below the original max health
        m_maxHealth *= Mathf.Max(1f, multiplier);
        m_currentHealth = m_maxHealth;
    }
}
