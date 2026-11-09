using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Enemy prefab and spawn points
    [Header("Enemy Spawner Settings")]
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private Transform[] m_spawnPoint;
    [SerializeField] private Transform[] m_waypoints; // Array of waypoints for the enemies to follow 

    // Enemy spawn settings
    [Header("Spawn Settings")]
    [SerializeField] private int m_TotalWaves = 5; // Total number of waves to spawn
    [SerializeField] private int m_startingEnemyCount = 5; // Starting number of enemies in the first wave
    [SerializeField] private int m_extraEnemiesPerWave = 2; // Additional enemies to spawn in each subsequent wave
    [SerializeField] private float m_spawnInterval = 2f; // Time interval between enemy spawns
    [SerializeField] private float m_timeBetweenWaves = 5f; // Time interval between waves

    // Reference to the player's base health
    [SerializeField] private Player m_playerBase;

    private int m_currentWave = 0; // Current wave number
    private int m_enemiesToSpawn = 0; // Number of enemies to spawn in the current wave
    private int m_spawnedEnemies = 0; // Number of enemies spawned in the current wave
    private int m_activeEnemies = 0; // Number of active enemies in the scene

    // private variables to keep track of the spawn timer and current enemy count
    private float m_spawnTimer = 0f;
    private float m_waveTimer = 0f;
    private float m_currentEnemyCount = 0;

    private bool m_waveActive; // Flag to indicate if a wave is currently active

    private void Start()
    {
        // Initialize the first wave
        StartNextWave();
    }

    private void Update()
    {
        //check if a wave is already active
        if (m_waveActive)
        {
            m_spawnTimer += Time.deltaTime;

            if (m_spawnedEnemies < m_enemiesToSpawn && m_spawnTimer >= m_spawnInterval)
            {
                SpawnEnemy();
                m_spawnTimer = 0f;
            }

            if (m_spawnedEnemies >= m_enemiesToSpawn && m_activeEnemies <= 0)
            {
                // Wave is complete, start the next wave after a delay
                m_waveActive = false;
                m_waveTimer = 0f;
            }

        }
        else if (m_currentWave < m_TotalWaves)
        {
            m_waveTimer += Time.deltaTime; // Increment the wave timer

            if (m_waveTimer >= m_timeBetweenWaves)
            {
                StartNextWave();
            }
        }
        else
        {
            //All waves have been completed
        }
    }

    private void StartNextWave()
    {
        m_currentWave++;

        // Reset the spawn timer and spawned enemies count for the new wave
        m_enemiesToSpawn = m_startingEnemyCount + (m_extraEnemiesPerWave * (m_currentWave - 1));

        m_spawnedEnemies = 0;
        m_spawnTimer = m_spawnInterval; // Reset the spawn timer to the spawn interval
        m_waveActive = true;

        Debug.Log($"Starting Wave {m_currentWave} with {m_enemiesToSpawn} enemies.");
    }

    private void SpawnEnemy()
    {
        //check if there are any spawn points available or if the enemy prefab is not assigned
        if (m_spawnPoint.Length == 0 || m_enemyPrefab == null)
        {
            Debug.LogWarning("No spawn points or enemy prefab assigned.");
            return;
        }

        //spawn enemy at a predefined spawn point
        GameObject enemy = Instantiate(m_enemyPrefab, m_spawnPoint[0].position, m_spawnPoint[0].rotation);

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();

        if (movement == null)
        {
            Debug.LogError("EnemySpawner: EnemyMovement is missing.");
            Destroy(enemy);
            return;
        }

        // Set the waypoints for the enemy to follow
        movement.SetWaypoints(m_waypoints);
        movement.SetBase(m_playerBase);

        // -- Add EnemyWaveTracker component to the spawned enemy if it doesn't already have one -- //
        EnemyWaveTracker tracker = enemy.GetComponent<EnemyWaveTracker>();

        if (tracker == null)
        {
            tracker = enemy.AddComponent<EnemyWaveTracker>();
        }

        tracker.SetSpawner(this);
        // -- //

        m_spawnedEnemies++;
        m_activeEnemies++;
    }

    public void EnemyRemoved()
    {
        m_activeEnemies = Mathf.Max(0, m_activeEnemies - 1);
    }
}
