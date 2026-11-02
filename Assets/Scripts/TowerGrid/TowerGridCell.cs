using UnityEngine;

public class TowerGridCell : MonoBehaviour
{
    private TowerGridUnit m_currentTower;

    public bool IsOccupied()
    {
        return m_currentTower != null;
    }

    public TowerGridUnit GetTower()
    {
        return m_currentTower;
    }

    public bool PlaceTower(TowerGridUnit tower)
    {
        if (tower == null || IsOccupied())
        {
            return false;
        }

        m_currentTower = tower;
        tower.SetCurrentCell(this);
        tower.transform.position = transform.position;
        return true;
    }

    public void RemoveTower()
    {
        if (m_currentTower != null)
        {
            m_currentTower.SetCurrentCell(null);
        }

        m_currentTower = null;
    }
}
