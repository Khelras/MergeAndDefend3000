using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // References to the EnemyHealth component and the UI elements
    [SerializeField] private EnemyHealth m_enemyHealth;
    [SerializeField] private Image m_healthFill;
    [SerializeField] private Camera m_mainCamera;

    private void LateUpdate()
    {
        // Ensure references are valid before proceeding.
        if (m_enemyHealth == null || m_healthFill == null)
        {
            return;
        }

        // Update bar based on remaining enemy health.
        m_healthFill.fillAmount = m_enemyHealth.GetHealthPercentage();

        // Make the bar face the camera.
        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        if (m_mainCamera != null)
        {
            transform.rotation = m_mainCamera.transform.rotation;
        }
    }
}
