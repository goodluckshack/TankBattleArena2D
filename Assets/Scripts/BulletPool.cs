using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int initialCapacity = 10;

    private List<GameObject> activeBullets = new List<GameObject>();
    private Queue<GameObject> bullets = new Queue<GameObject>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialCapacity; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        if (bullets.Count == 0)
        {
            ExpandPool();
        }
        GameObject bullet = bullets.Dequeue();
        bullet.SetActive(true);
        activeBullets.Add(bullet);
        return bullet;
    }

    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bullets.Enqueue(bullet);
        activeBullets.Remove(bullet);
    }

    void ExpandPool()
    {
        for (int i = 0; i < initialCapacity; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.SetActive(false);
            bullets.Enqueue(bullet);
        }
    }

    public void ReturnAllBullets()
    {
        foreach (GameObject bullet in new List<GameObject>(activeBullets))
        {
            ReturnBullet(bullet);
        }
    }
}