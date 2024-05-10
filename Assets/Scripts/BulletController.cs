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
            Debug.Log(gameObject.name + " self destroyed due to max distance");
            Destroy(gameObject);
        }
    }

    private void CheckCollision(float moveStep)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, moveStep);
        if (hit.collider != null && hit.collider.gameObject != shooter)
        {
            if (hit.collider.gameObject.GetComponent<PlayerTankController>() != null)
            {
                Destroy(hit.collider.gameObject);
                Debug.Log(hit.collider.gameObject + " destroyed. Game over!");
                Destroy(gameObject);
            }
            else if (hit.collider.gameObject.GetComponent<EnemyTankController>() != null)
            {
                ScoreManager.AddScore(100);
                Destroy(hit.collider.gameObject);
                Destroy(gameObject);
            }
        }
    }
}