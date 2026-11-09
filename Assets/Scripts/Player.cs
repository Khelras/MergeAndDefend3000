using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int m_currentHealth;
    private bool m_isDefeated;

    public bool IsDefeated
    {
        get { return m_isDefeated; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHealth = maxHealth;
        m_isDefeated = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage)
    {
        if (m_isDefeated || damage <= 0)
            return;

        m_currentHealth -= damage;
        m_currentHealth = Mathf.Max(m_currentHealth, 0); // Ensure health doesn't go below 0

        Debug.Log("BaseHealth: " + gameObject.name + " took " + damage + " damage. Current health: " + m_currentHealth);

        if (m_currentHealth <= 0)
        {
            m_isDefeated = true;
            Debug.Log($"{gameObject.name} has been defeated!");
        }
    }
}
