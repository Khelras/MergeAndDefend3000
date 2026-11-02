using UnityEngine;

public class TowerGridUnit : MonoBehaviour
{
    private TowerGridCell m_currentCell;

    public TowerGridCell GetCurrentCell()
    {
        return m_currentCell;
    }

    public void SetCurrentCell(TowerGridCell cell)
    {
        m_currentCell = cell;
    }
}
