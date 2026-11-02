using UnityEngine;

public class TowerMergeUnit : MonoBehaviour
{
    [SerializeField] private int m_towerTier = 1;

    public int GetTowerTier()
    {
        return m_towerTier;
    }
}
