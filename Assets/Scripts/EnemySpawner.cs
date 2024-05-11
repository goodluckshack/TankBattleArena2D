using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float _spawnRate = 5f;
    [SerializeField] private float _spawnRadius = 10f;
    [SerializeField] private int _maxEnemies = 5;

    private float nextSpawnTime;
    private int currentEnemyCount;

    void Start()
    {
        currentEnemyCount = 0;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime && currentEnemyCount < _maxEnemies)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + _spawnRate;
        }
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount < _maxEnemies)
        {
            GameObject enemy = EnemyTankPool.Instance.GetTank();
            enemy.transform.position = Random.insideUnitCircle * _spawnRadius + (Vector2)transform.position;
            enemy.SetActive(true);
            currentEnemyCount++;

            EnemyTankController enemyController = enemy.GetComponent<EnemyTankController>();
            enemyController.OnEnemyDestroyed += HandleEnemyDestroyed;
            if (PlayerTankController.Instance != null)
            {
                enemyController.player = PlayerTankController.Instance.transform;
            }

            EnemyGunController enemyGunController = enemy.GetComponent<EnemyGunController>();
            if (enemyGunController != null && PlayerTankController.Instance != null)
            {
                enemyGunController.player = PlayerTankController.Instance.transform;
            }
        }
    }

    private void HandleEnemyDestroyed()
    {
        currentEnemyCount--;
        if (currentEnemyCount < 0) currentEnemyCount = 0; // Обеспечиваем, что счетчик не уйдет в отрицательные значения
    }
}