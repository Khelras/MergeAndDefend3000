using UnityEngine;

public class TowerDragging : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private LayerMask m_towerLayer;
    [SerializeField] private LayerMask m_gridLayer;
    [SerializeField] private float m_dragHeight = 1.5f;

    private TowerGridUnit m_selectedTower;
    private TowerGridCell m_originalCell;
    private Plane m_dragPlane;

    private void Update()
    {
        // Check for mouse input to start dragging, continue dragging, or stop dragging the tower.
        if (Input.GetMouseButtonDown(0))
        {
            StartDragging();
        }

        if (Input.GetMouseButton(0))
        {
            DragTower();
        }

        if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
        }
    }

    private void StartDragging()
    {
        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_towerLayer))
        {
            TowerGridUnit tower = hit.collider.GetComponentInParent<TowerGridUnit>();

            if (tower == null || tower.GetCurrentCell() == null)
            {
                return;
            }

            m_selectedTower = tower;
            m_originalCell = tower.GetCurrentCell();

            // Keep the tower at a consistent height while dragging.
            m_dragPlane = new Plane(
                Vector3.up,
                new Vector3(0f, m_originalCell.transform.position.y + m_dragHeight, 0f)
            );
        }
    }

    private void DragTower()
    {
        if (m_selectedTower == null)
        {
            return;
        }

        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);

        if (m_dragPlane.Raycast(ray, out float distance))
        {
            Vector3 position = ray.GetPoint(distance);
            m_selectedTower.transform.position = position;
        }
    }

    private void StopDragging()
    {
        if (m_selectedTower == null)
        {
            return;
        }

        Ray ray = m_mainCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, m_gridLayer))
        {
            TowerGridCell targetCell = hit.collider.GetComponentInParent<TowerGridCell>();

            if (targetCell != null && targetCell == m_originalCell)
            {
                // Dropped back onto the original cell.
                m_selectedTower.transform.position = m_originalCell.transform.position;
            }
            else if (targetCell != null && !targetCell.IsOccupied())
            {
                // Move the tower into the empty cell.
                m_originalCell.RemoveTower();
                targetCell.PlaceTower(m_selectedTower);
            }
            else
            {
                ReturnTower();
            }
        }
        else
        {
            ReturnTower();
        }

        m_selectedTower = null;
        m_originalCell = null;
    }

    private void ReturnTower()
    {
        if (m_selectedTower != null && m_originalCell != null)
        {
            m_selectedTower.transform.position = m_originalCell.transform.position;
        }
    }
}
