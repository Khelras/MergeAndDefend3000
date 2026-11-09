using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    

    [Header("Health Stuffs")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private TMP_Text m_healthText;
    private int m_currentHealth;
    

    [Header("Money Stuffs")]
    [SerializeField] private int startingMoney = 100;
    [SerializeField] private TMP_Text m_moneyText;

    public int money { get; private set; }

    private bool m_isDefeated;

    public bool IsDefeated
    {
        get { return m_isDefeated; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHealth = maxHealth;
        UpdateHealthUI();

        m_isDefeated = false;

        money = startingMoney;
        UpdateMoneyUI();
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

        UpdateHealthUI();
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
            return;
        money += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0 || amount > money)
        {
            return false; // Not enough money to spend
        }

        money -= amount;
        UpdateMoneyUI();

        return true; // Successfully spent money
    }

    private void UpdateHealthUI()
    {
        if (m_healthText != null)
        {
            m_healthText.text = $"{m_currentHealth} / {maxHealth}";
        }
    }

    private void UpdateMoneyUI()
    {
        if (m_moneyText != null)
        {
            m_moneyText.text = $"${money}";
        }
    }
}
