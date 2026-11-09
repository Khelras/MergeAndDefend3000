using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Enemy Health Settings")]
    [SerializeField] private int maxHealth = 100;

    public float m_currentHealth;

    private void Start()
    {
        m_currentHealth = maxHealth;
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
        Debug.Log("Enemy has died.");
        // Add death logic here (e.g., play animation, drop loot, etc.)
        Destroy(gameObject);
        SFXManager.Instance.PlaySFX(SFXManager.Instance.EnemyDeathSFX, 1f);
    }
}
