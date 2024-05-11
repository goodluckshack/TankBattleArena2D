using System.Collections.Generic;
using UnityEngine;

public class EnemyTankPool : MonoBehaviour
{
    public static EnemyTankPool Instance { get; private set; }
    [SerializeField] private GameObject enemyTankPrefab;
    [SerializeField] private int initialCapacity = 10;

    private Queue<GameObject> enemyTanks = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialCapacity; i++)
        {
            GameObject tank = Instantiate(enemyTankPrefab);
            tank.SetActive(false);
            enemyTanks.Enqueue(tank);
        }
    }

    public GameObject GetTank()
    {
        if (enemyTanks.Count == 0)
        {
            ExpandPool();
        }
        GameObject tank = enemyTanks.Dequeue();
        tank.SetActive(true);
        return tank;
    }

    public void ReturnTank(GameObject tank)
    {
        tank.SetActive(false);
        enemyTanks.Enqueue(tank);
    }

    void ExpandPool()
    {
        for (int i = 0; i < initialCapacity; i++)
        {
            GameObject tank = Instantiate(enemyTankPrefab);
            tank.SetActive(false);
            enemyTanks.Enqueue(tank);
        }
    }
}