using UnityEngine;

public class EnemyTankController : MonoBehaviour, IEnemy
{
    public Transform player;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _detectionDistance = 30f;
    [SerializeField] private Animator trackARightAnimator;
    [SerializeField] private Animator trackALeftAnimator;

    private Rigidbody2D rb;
    private float _currentAngle;

    public delegate void EnemyDestroyedHandler();
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _currentAngle = transform.eulerAngles.z;
    }

    void FixedUpdate()
    {
        MoveTowardsPlayer();
        AnimateTracks();
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= _detectionDistance)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
                _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, targetAngle, _rotationSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(_currentAngle);
                rb.velocity = transform.up * _moveSpeed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void AnimateTracks()
    {
        float speedMagnitude = Mathf.Abs(rb.velocity.magnitude);
        trackARightAnimator.SetFloat("Speed", speedMagnitude);
        trackALeftAnimator.SetFloat("Speed", speedMagnitude);
    }

    void OnDestroy()
    {
        if (OnEnemyDestroyed != null)
        {
            OnEnemyDestroyed();
        }
    }
}
public interface IEnemy { }


