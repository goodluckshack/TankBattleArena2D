using System.Collections;
using System.Collections.Generic;
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
        Vector2 spawnPosition = Random.insideUnitCircle * _spawnRadius + (Vector2)transform.position;
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        enemy.SetActive(true);
        currentEnemyCount++;

        // Установка контроллера танка
        EnemyTankController enemyTankController = enemy.GetComponent<EnemyTankController>();
        enemyTankController.OnEnemyDestroyed += HandleEnemyDestroyed;
        if (PlayerTankController.Instance != null)
        {
            enemyTankController.player = PlayerTankController.Instance.transform;
        }

        // Установка контроллера оружия
        EnemyGunController enemyGunController = enemy.GetComponent<EnemyGunController>();
        if (enemyGunController != null && PlayerTankController.Instance != null)
        {
            enemyGunController.player = PlayerTankController.Instance.transform;
        }
    }


    private void HandleEnemyDestroyed()
    {
        currentEnemyCount--;
    }
}