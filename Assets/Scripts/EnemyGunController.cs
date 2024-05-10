using UnityEngine;

public class EnemyGunController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
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
        // Checking if the current time has surpassed the next allowable fire time and if the enemy can shoot the player
        if (player != null && CanShootAtPlayer())
        {
            if (Time.time >= _nextFireTime)
            {
                Shoot();
                _nextFireTime = Time.time + _reloadTime;
            }
        }
    }

    // This method handles the instantiation and setup of the bullet
    public void Shoot()
    {
        // Instantiating a bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.up * 1.4f, firePoint.rotation);

        // Set the shooter property of the bullet to this game object
        bullet.GetComponent<BulletController>().shooter = gameObject;

        Debug.Log("Bullet created by " + gameObject.name);
    }

    private bool CanShootAtPlayer()
    {
        //
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        //
        return distanceToPlayer <= _maxDistance;
    }
}