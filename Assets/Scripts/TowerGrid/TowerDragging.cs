using UnityEngine;
using UnityEngine.EventSystems;

public class TowerDragging : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private LayerMask m_towerLayer;
    [SerializeField] private LayerMask m_gridLayer;

    [Header("Settings")]
    [SerializeField] private float m_dragHeight = 1.5f;

    private TowerGridUnit m_selectedTower;
    private TowerGridCell m_originalCell;
    private TowerGridCell m_hoveredCell;
    private Plane m_dragPlane;

    private void Update()
    {
        if (m_mainCamera == null)
        {
            return;
        }

        Vector2 screenPosition;
        bool pressed = false;
        bool held = false;
        bool released = false;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            screenPosition = touch.position;

            pressed = touch.phase == TouchPhase.Began;
            held = touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary;
            released = touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled;
        }
        else
        {
            screenPosition = Input.mousePosition;

            pressed = Input.GetMouseButtonDown(0);
            held = Input.GetMouseButton(0);
            released = Input.GetMouseButtonUp(0);
        }

        if (pressed)
        {
            StartDragging(screenPosition);
        }

        if (held)
        {
            DragTower(screenPosition);
        }

        if (released)
        {
            StopDragging(screenPosition);
        }
    }

    private void StartDragging(Vector2 screenPosition)
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        Ray ray = m_mainCamera.ScreenPointToRay(screenPosition);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, m_towerLayer))
        {
            return;
        }

        TowerGridUnit tower = hit.collider.GetComponentInParent<TowerGridUnit>();

        if (tower == null || tower.GetCurrentCell() == null)
        {
            return;
        }

        m_selectedTower = tower;
        m_originalCell = tower.GetCurrentCell();

        m_dragPlane = new Plane( Vector3.up, new Vector3( 0f, m_originalCell.transform.position.y + m_dragHeight, 0f ));
    }

    private void DragTower(Vector2 screenPosition)
    {
        if (m_selectedTower == null)
        {
            return;
        }

        Ray ray = m_mainCamera.ScreenPointToRay(screenPosition);

        if (m_dragPlane.Raycast(ray, out float distance))
        {
            m_selectedTower.transform.position = ray.GetPoint(distance);
        }

        UpdateHoveredCell(screenPosition);
    }

    private void UpdateHoveredCell(Vector2 screenPosition)
    {
        TowerGridCell newCell = GetCellAtScreenPosition(screenPosition);

        if (newCell != m_hoveredCell)
        {
            if (m_hoveredCell != null)
            {
                m_hoveredCell.SetHighlight(false);
            }

            m_hoveredCell = newCell;
            if (m_hoveredCell != null)
            {
                bool validCell = !m_hoveredCell.IsOccupied() || m_hoveredCell == m_originalCell;

                m_hoveredCell.SetHighlight(validCell);
            }
        }
    }

    private TowerGridCell GetCellAtScreenPosition(Vector2 screenPosition)
    {
        Ray ray = m_mainCamera.ScreenPointToRay(screenPosition);

        RaycastHit[] hits = Physics.RaycastAll(
            ray,
            1000f,
            m_gridLayer
        );

        foreach (RaycastHit hit in hits)
        {
            TowerGridCell cell =
                hit.collider.GetComponentInParent<TowerGridCell>();

            if (cell != null)
            {
                return cell;
            }
        }

        return null;
    }

    private void StopDragging(Vector2 screenPosition)
    {
        if (m_selectedTower == null)
        {
            return;
        }

        TowerGridCell targetCell =
            GetCellAtScreenPosition(screenPosition);

        if (targetCell != null && targetCell == m_originalCell)
        {
            ReturnTower();
        }
        else if (targetCell != null && !targetCell.IsOccupied())
        {
            m_originalCell.RemoveTower();
            targetCell.PlaceTower(m_selectedTower);
        }
        else
        {
            ReturnTower();
        }

        if (m_hoveredCell != null)
        {
            m_hoveredCell.SetHighlight(false);
        }

        m_selectedTower = null;
        m_originalCell = null;
        m_hoveredCell = null;
    }

    private void ReturnTower()
    {
        if (m_selectedTower != null && m_originalCell != null)
        {
            m_selectedTower.transform.position = m_originalCell.transform.position;
        }
    }
}
