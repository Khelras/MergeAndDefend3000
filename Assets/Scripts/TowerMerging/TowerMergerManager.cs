using UnityEngine;

public class TowerMergerManager : MonoBehaviour
{
    [Header("Tower Prefabs")]
    [SerializeField] private TowerGridUnit m_tier2Prefab;
    [SerializeField] private TowerGridUnit m_tier3Prefab;
    [SerializeField] private TowerGridUnit m_tier4Prefab;
    [SerializeField] private TowerGridUnit m_tier5Prefab;

    private const int MAX_TOWER_TIER = 5; // Maximum tower tier allowed

    public bool CanMerge(TowerGridUnit firstTower, TowerGridUnit secondTower)
    {
        // Check if both towers are valid and not the same instance.
        if (firstTower == null || secondTower == null || firstTower == secondTower)
        {
            return false;
        }

        // Check if both towers are in the same cell.
        TowerMergeUnit firstMerge = firstTower.GetComponent<TowerMergeUnit>();
        TowerMergeUnit secondMerge = secondTower.GetComponent<TowerMergeUnit>();

        // Ensure both towers have the TowerMergeUnit component and are of the same tier.
        if (firstMerge == null || secondMerge == null)
        {
            return false;
        }
        // Check if both towers are of the same tier and within the allowed range for merging.
        int tier = firstMerge.GetTowerTier();

        return tier == secondMerge.GetTowerTier() && tier >= 1 && tier < MAX_TOWER_TIER;
    }

    public bool MergeTowers(TowerGridUnit draggedTower, TowerGridUnit targetTower)
    {
        // Validate if the towers can be merged.
        if (!CanMerge(draggedTower, targetTower))
        {
            return false;
        }

        // Get the cells of both towers.
        TowerGridCell sourceCell = draggedTower.GetCurrentCell();
        TowerGridCell targetCell = targetTower.GetCurrentCell();

        // Ensure both cells are valid and not the same.
        if (sourceCell == null || targetCell == null || sourceCell == targetCell)
        {
            return false;
        }

        // Determine the next tier prefab based on the current tier of the dragged tower.
        int currentTier = draggedTower.GetComponent<TowerMergeUnit>().GetTowerTier();
        TowerGridUnit nextPrefab = null;

        switch (currentTier)
        {
            case 1:
                nextPrefab = m_tier2Prefab;
                break;
            case 2:
                nextPrefab = m_tier3Prefab;
                break;
            case 3:
                nextPrefab = m_tier4Prefab;
                break;
            case 4:
                nextPrefab = m_tier5Prefab;
                break;
            default:
                Debug.LogError("TowerMergerManager: Invalid tower tier for merging.");
                return false;
        }

        // Ensure the next tier prefab is valid.
        if (nextPrefab == null)
        {
            Debug.LogError("TowerMergerManager: Missing next tier prefab."); 
            return false;
        }

        // Instantiate the new merged tower.
        TowerGridUnit mergedTower = Instantiate(nextPrefab);

        // Remove both original towers from their cells.
        sourceCell.RemoveTower();
        targetCell.RemoveTower();

        // Place the upgraded tower in the target cell.
        if (!targetCell.PlaceTower(mergedTower))
        {
            Destroy(mergedTower.gameObject);
            Debug.LogError("TowerMergerManager: Failed to place merged tower.");
            return false;
        }

        Destroy(draggedTower.gameObject);
        Destroy(targetTower.gameObject);

        return true;
    }
}
