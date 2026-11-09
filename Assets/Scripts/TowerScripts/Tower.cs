using System.Collections;
using UnityEngine;
using System.Collections.Generic;
public enum TowerTargetPriority 
{
    FIRST,
    CLOSE,
    STRONG
}
public class Tower : MonoBehaviour
{
    [Header("Info")]
    protected List<EnemyHealth> m_currentEnemiesInRange = new List<EnemyHealth>();
    public EnemyHealth m_currentEnemy;
    public TowerTargetPriority m_targetPriority;
    [SerializeField] Transform m_head;
    [SerializeField] float m_rotationSpeed = 1.0f;
    public bool m_rotateTowardsTarget;


    [Header("attacking")]
    public float m_attakingRate;
    private float m_lastAttackTime;
    public GameObject m_projectilePrefab;
    private Transform m_projectileSpawnPos;

    public int m_projectileDamaged;
    public float m_projectileSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - m_lastAttackTime > m_attakingRate) 
        {
            m_lastAttackTime = Time.time;
            m_currentEnemy = GetEenemy();
            if (m_currentEnemy != null) 
            {
                Attack();
            }
        }
        if (m_currentEnemy != null) 
        {
            TrackEnemy();
        }
    }
    private EnemyHealth GetEenemy()
    {
        // the enemy => enemy == null is just a lambda experssion I have no idea how lambda expersion works I just copy this in the article :(
        // this just to remove enemy in the list that are null so we dont get a null reference when the enemy dies inside the range of the tower by another tower
        m_currentEnemiesInRange.RemoveAll(enemy => enemy == null);

        if (m_currentEnemiesInRange.Count == 0) 
        {
            return null;
        }
        if (m_currentEnemiesInRange.Count == 1) 
        {
            return m_currentEnemiesInRange[0];
        }
        switch (m_targetPriority) 
        {
            case TowerTargetPriority.FIRST:

                return m_currentEnemiesInRange[0];

            case TowerTargetPriority.CLOSE:
                EnemyHealth closest = null;
                float shorterDistance = 99;
                for (int i = 0; i < m_currentEnemiesInRange.Count; i++) {

                    float distnace = (transform.position - m_currentEnemiesInRange[i].transform.position).sqrMagnitude;

                    if (distnace < shorterDistance) 
                    {
                        closest = m_currentEnemiesInRange[i];
                        shorterDistance = distnace;
                    }
                }
                return closest;
            case TowerTargetPriority.STRONG:
                EnemyHealth strongest = null;
                float strongestHealth = 0;

                foreach (EnemyHealth enemy in m_currentEnemiesInRange) 
                {
                    if (enemy.m_currentHealth > strongestHealth) 
                    {
                        strongest = enemy;
                        strongestHealth = enemy.m_currentHealth;
                    }
                }
                return strongest;

        }
        return null;
    }
    public virtual void Attack() 
    {

        GameObject bullet = Instantiate(m_projectilePrefab, m_projectileSpawnPos.position, Quaternion.identity);
        bullet.GetComponent<Projectile>().Initialize(m_currentEnemy, m_projectileDamaged, m_projectileSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")) 
        {
            m_currentEnemiesInRange.Add(other.GetComponent<EnemyHealth>());
            //TrackEnemy();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            m_currentEnemiesInRange.Remove(other.GetComponent<EnemyHealth>());
        }
    }
    public void TrackEnemy()
    {
        if (m_rotateTowardsTarget) 
        {
            Vector3 direction = (m_currentEnemy.transform.position - m_head.position).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            m_head.rotation = Quaternion.Slerp(m_head.rotation, rotation, m_rotationSpeed * Time.deltaTime);
        }
   
    }
}

