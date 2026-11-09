using TMPro;
using UnityEditor;
using UnityEngine;
using TMPro; 

public class Player : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;

    private int m_currentHealth;
    private bool m_isDefeated;

    [Header("Money Stuffs")]
    [SerializeField] private int startingMoney = 100;
    [SerializeField] private TMP_Text m_moneyText;

    public int money { get; private set; }

    public bool IsDefeated
    {
        get { return m_isDefeated; }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_currentHealth = maxHealth;
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

    private void UpdateMoneyUI()
    {
        if (m_moneyText != null)
        {
            m_moneyText.text = $"Money: {money}";
        }
    }
}
