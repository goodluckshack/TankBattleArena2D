using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
    public static PlayerTankController Instance { get; private set; }
    public const int VIEW_DISTANCE = 15;

    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 150f;
    [SerializeField] private JoystickControl joystickControl;
    [SerializeField] private float _dumpingSpeed;
    [SerializeField] private Camera _camera;
    [SerializeField] private BattleArenaGenerator mapGenerator;

    private Rigidbody2D rb;
    private Vector2 movementInput;
    private Animator trackARightAnimator;
    private Animator trackALeftAnimator;
    private Vector2Int lastGeneratedPosition;
    private Vector2 currentVelocity;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trackARightAnimator = transform.Find("TrackARight").GetComponent<Animator>();
        trackALeftAnimator = transform.Find("TrackALeft").GetComponent<Animator>();
        Vector2Int playerPos = new(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        PlayerMoved(playerPos);
    }

    private float _distance = 1f;

    void Update()
    {
        ReadInput();
        TrackPlayerPosition();
    }

    void FixedUpdate()
    {
        // Call the Move function to update position and rotation
        Move();
        AnimateTracks();
        _camera.transform.position = Vector3.Lerp(new Vector3(_camera.transform.position.x, _camera.transform.position.y + 0.125f, -10), transform.position, Time.deltaTime * _dumpingSpeed);
    }

    private void ReadInput()
    {
        // Read the vertical and horizontal input from the joystick
        float verticalInput = joystickControl.Vertical();
        float horizontalInput = joystickControl.Horizontal();
        movementInput = new Vector2(horizontalInput, verticalInput);
    }

    private void Move()
    {
        // Check if there is any input
        if (movementInput != Vector2.zero)
        {
            Vector2 movementDirection = movementInput.normalized;

            // Calculate the target angle for rotation towards the input direction
            float targetAngle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg - 90;

            // Smoothly rotate towards the target angle at the specified rotation speed
            float angle = Mathf.MoveTowardsAngle(rb.rotation, targetAngle, _rotationSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(angle);

            //// Calculate the vector for movement
            //Vector2 movement = movementDirection * _moveSpeed * Time.fixedDeltaTime;

            //// Move the tank to the new position
            //rb.MovePosition(rb.position + movement);

            Vector2 targetVelocity = movementDirection * _moveSpeed;
            currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, Time.fixedDeltaTime * _moveSpeed);
            rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
        }
        else
        {
            // Gradually reduce the velocity to zero when there is no input
            currentVelocity = Vector2.Lerp(currentVelocity, Vector2.zero, Time.fixedDeltaTime * _moveSpeed);
            rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
        }
    }

    // Tank track animation
    private void AnimateTracks()
    {
        float speedMagnitude = movementInput.sqrMagnitude;
        trackARightAnimator.SetFloat("Speed", speedMagnitude);
        trackALeftAnimator.SetFloat("Speed", speedMagnitude);
    }

    // Map generation
    private void TrackPlayerPosition()
    {
        Vector2Int playerPos = new(Mathf.FloorToInt(transform.position.x), Mathf.FloorToInt(transform.position.y));
        if (Vector2Int.Distance(playerPos, lastGeneratedPosition) > _distance)
        {
            PlayerMoved(playerPos);
        }
    }

    private void PlayerMoved(Vector2Int playerPos)
    {
        GameEventsManager.Instance.MapEvents.PlayerMoved(playerPos);
        lastGeneratedPosition = playerPos;
    }


    void OnDestroy()
    {
        if (BulletPool.Instance != null)
        {
            BulletPool.Instance.ReturnAllBullets();
        }
    }
}

