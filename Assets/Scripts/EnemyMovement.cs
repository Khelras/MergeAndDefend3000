using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Serialized fields for waypoints and speed
    [SerializeField] private Transform[] m_waypoints;
    [SerializeField] private float m_speed = 2f;

    //Store the basehp and how much damage the enemy will do to the base
    [SerializeField] private int m_baseDamage = 10;

    private Player m_playerBase;

    // private int to keep track of the current waypoint index
    private int m_currentWaypoint = 0;

    public void SetWaypoints(Transform[] waypoints)
    {
        m_waypoints = waypoints;
        m_currentWaypoint = 0; // Reset the current waypoint index when new waypoints are set
    }

    private void Update()
    {
        if (m_waypoints == null || m_waypoints.Length == 0)
        {
            return; // No waypoints to follow
        }

        // Get the current target waypoint
        Transform target = m_waypoints[m_currentWaypoint];

        transform.position = Vector3.MoveTowards(transform.position, target.position, m_speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            m_currentWaypoint++;
        }

        // Check if the enemy has reached the player's base
        if (m_currentWaypoint >= m_waypoints.Length && m_playerBase != null)
        {
            m_playerBase.TakeDamage(m_baseDamage);
            Destroy(gameObject); // Destroy the enemy after dealing damage to the base
            return; // Exit the Update method to prevent further movement after reaching the base
        }
        else if (m_currentWaypoint >= m_waypoints.Length)
        {
            // If the enemy has reached the last waypoint but the base is not set, destroy the enemy
            Destroy(gameObject);
            return; // Exit the Update method to prevent further movement after reaching the last waypoint
        }

    }

    public void SetBase(Player playerBase)
    {
        m_playerBase = playerBase;
    }
}
