using UnityEngine;
using UnityEngine.UI;

public class PlayerGunController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float _reloadTime = 2f;
    [SerializeField] private float initialDelay = 3f;

    [SerializeField] private Button _shootButton;
    private float _lastShotTime;

    void Start()
    {
        _shootButton = GetComponent<Button>();
        _lastShotTime = Time.time + initialDelay;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonPress()
    {
        Shoot();
    }

    public void Shoot()
    {
        if (Time.time - _lastShotTime > _reloadTime)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position + firePoint.up * 1.4f, firePoint.rotation);

            // Set the shooter property of the bullet to this game object
            bullet.GetComponent<BulletController>().shooter = gameObject;

            _lastShotTime = Time.time;
        }
    }
}