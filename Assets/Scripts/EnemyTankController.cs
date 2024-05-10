using UnityEngine;

public class EnemyTankController : MonoBehaviour
{
    public Transform player;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _rotationSpeed = 300f;
    [SerializeField] private float _detectionDistance = 30f;

    private Rigidbody2D rb;
    private Animator trackARightAnimator;
    private Animator trackALeftAnimator;
    private float _currentAngle;

    public delegate void EnemyDestroyedHandler();
    public event EnemyDestroyedHandler OnEnemyDestroyed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trackARightAnimator = transform.Find("TrackARight").GetComponent<Animator>();
        trackALeftAnimator = transform.Find("TrackALeft").GetComponent<Animator>();
        _currentAngle = transform.eulerAngles.z;        
    }

    void FixedUpdate()
    {
        // Handling movement towards the player
        MoveTowardsPlayer();

        // Updating track animations based on movement
        AnimateTracks();
    }

    private void MoveTowardsPlayer()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= _detectionDistance)
            {
                // Only move towards the player if within activation distance
                Vector2 direction = (player.position - transform.position).normalized;
                float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

                // Smoothly rotate towards the target angle using linear interpolation
                _currentAngle = Mathf.MoveTowardsAngle(_currentAngle, targetAngle, _rotationSpeed * Time.fixedDeltaTime);
                rb.MoveRotation(_currentAngle);

                // Move the tank forward along its current up vector
                rb.velocity = transform.up * _moveSpeed;
            }
            else
            {
                // If the player is too far, stop moving
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
            OnEnemyDestroyed();
    }
}

