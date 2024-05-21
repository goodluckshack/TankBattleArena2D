using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _maxDistance = 10f;

    public GameObject shooter;
    private Vector2 _startPosition;
    private float traveledDistance = 0f;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        float moveStep = _speed * Time.deltaTime;
        CheckCollision(moveStep);
        transform.Translate(Vector2.up * moveStep);
        traveledDistance += moveStep;

        if (traveledDistance >= _maxDistance)
        {
            BulletPool.Instance.ReturnBullet(gameObject);
        }
    }

    private void CheckCollision(float moveStep)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveStep);
        if (hit.collider != null && hit.collider.gameObject != shooter)
        {
            if (shooter.GetComponent<EnemyTankController>() != null && hit.collider.GetComponent<EnemyTankController>() != null)
            {
                BulletPool.Instance.ReturnBullet(gameObject);
                return;
            }

            var playerTankController = hit.collider.gameObject.GetComponent<PlayerTankController>();
            if (playerTankController != null)
            {
                Destroy(hit.collider.gameObject);
                Debug.Log(playerTankController.name + " destroyed. Game over!");
                BulletPool.Instance.ReturnBullet(gameObject);
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                return;
            }

            var enemyTankController = hit.collider.gameObject.GetComponent<EnemyTankController>();
            if (enemyTankController != null)
            {
                ScoreManager.AddScore(100);
                Destroy(hit.collider.gameObject);
                BulletPool.Instance.ReturnBullet(gameObject);
            }
        }
    }

}