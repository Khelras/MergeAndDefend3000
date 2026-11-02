using UnityEngine;

public class TowerGridTester : MonoBehaviour
{
    [SerializeField] private TowerGridCell m_testCell;
    [SerializeField] private TowerGridUnit m_towerPrefab;

    private void Start()
    {
        if (m_testCell == null || m_towerPrefab == null)
        {
            Debug.LogError("Missing tower or grid cell.");
            return;
        }

        if (m_testCell.IsOccupied())
        {
            return;
        }

        TowerGridUnit tower = Instantiate(m_towerPrefab);

        if (!m_testCell.PlaceTower(tower))
        {
            Destroy(tower.gameObject);
        }
    }
}
