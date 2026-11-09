using UnityEngine;

public class TowerSummoner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Player m_player;
    [SerializeField] private TowerGridCell[] m_gridCells;
    [SerializeField] private TowerGridUnit m_tier1Prefab;

    [Header("Settings")]
    [SerializeField] private int m_towerCost = 50;

    public void SummonTower()
    {
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
