using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _airControl = 2f;
    [SerializeField] private float _characterRadius = 0.4f;

    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _gravity = 15f;
    [SerializeField] private float _maxFallSpeed = 20f;

    [SerializeField] private bool _rotateToSurface = true;
    [SerializeField] private float _rotationSpeed = 10f;

    private Collider2D _platform;

    private IInputProvider _inputProvider;
    private Vector2 _velocity;
    private bool _isGrounded;
    private bool _isJumping;


    public void Initialize(IInputProvider inputProvider, Collider2D platform)
    {
        _inputProvider = inputProvider;
        _platform = platform;
    }

    void Update()
    {
        if (_inputProvider == null) 
            return;

        float moveInput = _inputProvider.GetMoveInput();
        bool jumpPressed = _inputProvider.GetJumpPressed();

        if (jumpPressed)
            _inputProvider.ClearJump();

        Vector2 currentPos = transform.position;
        Vector2 closestPoint = _platform.ClosestPoint(currentPos);
        float distanceToPlatform = Vector2.Distance(currentPos, closestPoint);
        float groundTolerance = 0.05f;
        _isGrounded = distanceToPlatform <= _characterRadius + groundTolerance;

        if (_isGrounded && !_isJumping)
        {
            Movement(moveInput);
        }

        if (jumpPressed && _isGrounded && !_isJumping)
        {
            Vector2 normal = (currentPos - closestPoint).normalized;
            _velocity = normal * _jumpForce;
            _isJumping = true;
        }

        if (_rotateToSurface)
            UpdateCharacterRotation();
    }

    void FixedUpdate()
    {
        if (_inputProvider == null) 
            return;

        if (_isJumping || !_isGrounded)
        {
            GravityAndMove();
            AirMovement(_inputProvider.GetMoveInput());
        }
    }

    void Movement(float moveInput)
    {
        Vector2 currentPos = transform.position;
        Vector2 closestPoint = _platform.ClosestPoint(currentPos);
        Vector2 normal = (currentPos - closestPoint).normalized;
        Vector2 tangent = new Vector2(normal.y, -normal.x);

        transform.position += (Vector3)(tangent * moveInput * _speed * Time.deltaTime);

        Vector2 newPos = transform.position;
        Vector2 newClosest = _platform.ClosestPoint(newPos);
        Vector2 newNormal = (newPos - newClosest).normalized;
        transform.position = newClosest + newNormal * _characterRadius;
        _velocity = Vector2.zero;
    }

    void AirMovement(float moveInput)
    {
        if (Mathf.Abs(moveInput) < 0.01f)
            return;

        Vector2 currentPos = transform.position;
        Vector2 closestPoint = _platform.ClosestPoint(currentPos);
        Vector2 gravityDirection = (closestPoint - currentPos).normalized;
        Vector2 tangent = new Vector2(-gravityDirection.y, gravityDirection.x);

        _velocity += tangent * moveInput * _airControl * Time.fixedDeltaTime;

        float horizontalSpeed = Vector2.Dot(_velocity, tangent);
        float airSpeedMultiplier = 0.8f;
        float maxAirSpeed = _speed * airSpeedMultiplier;
        if (Mathf.Abs(horizontalSpeed) > maxAirSpeed)
        {
            _velocity -= tangent * (horizontalSpeed - Mathf.Sign(horizontalSpeed) * maxAirSpeed);
        }
    }

    void GravityAndMove()
    {
        Vector2 currentPos = transform.position;
        Vector2 closestPoint = _platform.ClosestPoint(currentPos);
        Vector2 gravityDirection = (closestPoint - currentPos).normalized;

        _velocity += gravityDirection * _gravity * Time.fixedDeltaTime;
        if (_velocity.magnitude > _maxFallSpeed) _velocity = _velocity.normalized * _maxFallSpeed;

        transform.position += (Vector3)_velocity * Time.fixedDeltaTime;

        Vector2 newPos = transform.position;
        Vector2 newClosest = _platform.ClosestPoint(newPos);
        float groundTolerance = 0.05f;
        if (Vector2.Distance(newPos, newClosest) <= _characterRadius + groundTolerance)
        {
            Vector2 normal = (newPos - newClosest).normalized;
            transform.position = newClosest + normal * _characterRadius;
            _isJumping = false;
            _velocity = Vector2.zero;
        }
    }

    void UpdateCharacterRotation()
    {
        Vector2 closestPoint = _platform.ClosestPoint(transform.position);
        Vector2 normal = (transform.position - (Vector3)closestPoint).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, normal);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), _rotationSpeed * Time.deltaTime);
    }
}
