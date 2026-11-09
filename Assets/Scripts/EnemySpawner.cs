using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner")]
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private Transform[] m_spawnPoint;
    [SerializeField] private Transform[] m_waypoints;
    [SerializeField] private Player m_playerBase;

    [Header("Wave Difficulty")]
    [SerializeField] private int m_startingEnemyCount = 5;
    [SerializeField] private int m_extraEnemiesPerWave = 2;
    [SerializeField] private float m_startingSpawnInterval = 2f;
    [SerializeField] private float m_spawnIntervalDecrease = 0.1f;
    [SerializeField] private float m_minSpawnInterval = 0.5f;
    [SerializeField] private float m_healthIncreasePerWave = 0.2f;
    [SerializeField] private float m_timeBetweenWaves = 5f;

    [Header("UI")]
    [SerializeField] private TMP_Text m_waveText;
    [SerializeField] private TMP_Text m_highScoreText;

    private const string HIGH_SCORE_KEY = "EndlessWaveHighScore";

    private int m_currentWave;
    private int m_highScore;
    private int m_enemiesToSpawn;
    private int m_spawnedEnemies;
    private int m_activeEnemies;

    private float m_spawnInterval;
    private float m_spawnTimer;
    private float m_waveTimer;
    private float m_healthMultiplier;

    private bool m_waveActive;

    private void Start()
    {
        m_highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);

        UpdateUI();
        // Initialize the first wave
        StartNextWave();
    }

    private void Update()
    {
        // if the player base is null or defeated, we should not continue spawning enemies
        if (m_playerBase == null || m_playerBase.IsDefeated)
        {
            return;
        }

        //if the wave is active, we should spawn enemies based on the spawn interval and check if the wave is complete
        if (m_waveActive)
        {
            m_spawnTimer += Time.deltaTime;

            // Spawn enemies based on the spawn interval
            if (m_spawnedEnemies < m_enemiesToSpawn && m_spawnTimer >= m_spawnInterval)
            {
                SpawnEnemy();
                m_spawnTimer = 0f;
            }

            if (m_spawnedEnemies >= m_enemiesToSpawn && m_activeEnemies <= 0)
            {
                CompleteWave();
            }
        }
        else //if the wave is not active, we should check if it's time to start the next wave based on the time between waves
        {
            m_waveTimer += Time.deltaTime;

            // Start the next wave if the time between waves has passed
            if (m_waveTimer >= m_timeBetweenWaves)
            {
                StartNextWave();
            }
        }
    }

    private void StartNextWave()
    {
        m_currentWave++;

        // Calculate the number of enemies to spawn, spawn interval, and health multiplier for the current wave
        m_enemiesToSpawn = m_startingEnemyCount + m_extraEnemiesPerWave * (m_currentWave - 1);

        // Ensure the spawn interval does not go below the minimum spawn interval
        m_spawnInterval = Mathf.Max(m_minSpawnInterval, m_startingSpawnInterval - m_spawnIntervalDecrease * (m_currentWave - 1));

        m_healthMultiplier = 1f + m_healthIncreasePerWave * (m_currentWave - 1);

        // Reset wave-related variables
        m_spawnedEnemies = 0;
        m_activeEnemies = 0;
        m_spawnTimer = m_spawnInterval;
        m_waveActive = true;

        // Update the UI to reflect the new wave
        UpdateUI();
    }

    private void SpawnEnemy()
    {
        //init ialize the enemy prefab and set its health based on the current wave's health multiplier
        GameObject enemy = Instantiate(m_enemyPrefab, m_spawnPoint[0].position, m_spawnPoint[0].rotation);

        EnemyMovement movement = enemy.GetComponent<EnemyMovement>();
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();

        //Set all the necessary references for the enemy's movement and health components
        movement.SetWaypoints(m_waypoints);
        movement.SetBase(m_playerBase);

        health.SetPlayerReference(m_playerBase);
        health.SetHealthMultiplier(m_healthMultiplier);

        // Add an EnemyWaveTracker component to the enemy to track its spawner and wave information
        EnemyWaveTracker tracker = enemy.GetComponent<EnemyWaveTracker>();

        if (tracker == null)
        {
            tracker = enemy.AddComponent<EnemyWaveTracker>();
        }

        // Set the spawner reference in the EnemyWaveTracker to this spawner
        tracker.SetSpawner(this);

        m_spawnedEnemies++;
        m_activeEnemies++;
    }

    private void CompleteWave()
    {
        m_waveActive = false;
        m_waveTimer = 0f;

        // if the current wave exceeds the high score, update the high score and save it to PlayerPrefs
        if (m_currentWave > m_highScore)
        {
            m_highScore = m_currentWave;
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, m_highScore);
            PlayerPrefs.Save();
        }

        UpdateUI();
    }

    public void EnemyRemoved()
    {
        m_activeEnemies = Mathf.Max(0, m_activeEnemies - 1);
    }

    private void UpdateUI()
    {
        if (m_waveText != null)
        {
            m_waveText.text = $"Wave: {m_currentWave}";
        }
        if (m_highScoreText != null)
        {
            m_highScoreText.text = $"High Score: {m_highScore}";
        }
    }
}
