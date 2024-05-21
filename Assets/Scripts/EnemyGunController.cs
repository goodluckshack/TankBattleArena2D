using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    public Transform player;
    [SerializeField] private float _reloadTime = 2f;
    [SerializeField] private float _initialDelay = 3f;
    private float _nextFireTime = 0f;

    [SerializeField] private float _maxDistance = 10f;

    void Start()
    {
        _nextFireTime = Time.time + _initialDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && CanShootAtPlayer())
        {
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + _reloadTime;
            }
        }
    }


    public void Shoot()
    {
        if (BulletPool.Instance != null)
        {
            GameObject bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = firePoint.position + firePoint.up * 1.4f;
            bullet.transform.rotation = firePoint.rotation;
            bullet.GetComponent<BulletController>().shooter = gameObject;
            bullet.SetActive(true);

            Debug.Log("Bullet created by " + gameObject.name + " using pool.");
        }
    }

    private bool CanShootAtPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return distanceToPlayer <= _maxDistance;
    }
}