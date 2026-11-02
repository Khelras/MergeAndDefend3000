using UnityEngine;

public class TowerMergerManager : MonoBehaviour
{
    [Header("Tower Prefabs")]
    [SerializeField] private TowerGridUnit m_tier2Prefab;
    [SerializeField] private TowerGridUnit m_tier3Prefab;

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
        // Check if both towers are of the same tier and not already at the maximum tier (3).
        int tier = firstMerge.GetTowerTier();
        return tier == secondMerge.GetTowerTier() && tier < 3;
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
        TowerGridUnit nextPrefab = currentTier == 1 ? m_tier2Prefab : m_tier3Prefab;

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
