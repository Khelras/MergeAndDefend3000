using UnityEngine;

public class TowerSummoner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player m_player;
    [SerializeField] private TowerGridUnit m_tier1Prefab;
    [SerializeField] private Transform m_towerGridParent;

    private TowerGridCell[] m_gridCells;

    [Header("Settings")]
    [SerializeField] private int m_towerCost = 50;

    private void Awake()
    {
        if (m_towerGridParent == null)
        {
            Debug.LogError("Tower grid parent is not assigned in TowerSummoner.");
            return;
        }

        m_gridCells = m_towerGridParent.GetComponentsInChildren<TowerGridCell>();

        Debug.Log($"Found {m_gridCells.Length} grid cells for tower placement.");
    }

    public void SummonTower()
    {
        Debug.Log("SUMMON BUTTON CLICKED!");

        if (m_player == null || m_tier1Prefab == null)
        {
            Debug.LogError("Player or Tower Prefab reference is missing in TowerSummoner.");
            return;
        }

        if (m_player.IsDefeated)
        {
            Debug.Log("Cannot summon tower: Player is defeated.");
            return;
        }

        // Find the first grid cell not taking
        TowerGridCell availableCell = null;

        foreach (TowerGridCell cell in m_gridCells)
        {
            if (cell != null && !cell.IsOccupied())
            {
                availableCell = cell;
                break;
            }
        }

        if (availableCell == null)
        {
            Debug.Log("No available grid cells to summon a tower.");
            return;
        }

        // Spend money only if the player can afford it 
        if (!m_player.SpendMoney(m_towerCost))
        {
            Debug.Log("Cannot summon tower: Not enough money.");
            return;
        }

        // Summon the tower
        TowerGridUnit newTower = Instantiate(m_tier1Prefab);

        if (!availableCell.PlaceTower(newTower))
        {
            Destroy(newTower.gameObject);

            //Refund the player if placement fails
            m_player.AddMoney(m_towerCost);

            Debug.Log("Failed to place tower on the grid cell. Money refunded.");
            return;
        }

        Debug.Log("Tower summoned successfully.");
    }
}
