using UnityEngine;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerStat))]
public class PlayerMove : MonoBehaviour
{
    private CharacterController _controller;
    private PlayerStat _stat;
    private Camera _mainCamera;
    
    [Header("Movement")]
    [Space]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _runningSpeed;
    [SerializeField] private float _runStaminaUsage;
    private float _staminaUseTime;
    
    [Header("Jump")]
    [Space]
    [SerializeField] private float _jumpPower;
    [SerializeField] private float _jumpStaminaUsage;
    [SerializeField] private float _maxJumpCount;
    private int _jumpCounter;
    private const float Gravity = -9.81f;
    private float _yVelocity;    // 중력에 의해 누적될 y값 변수
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stat = GetComponent<PlayerStat>();
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        Jump();
        Move();
    }

    private float GetSpeed()
    {
        bool canRun = Input.GetKey(KeyCode.LeftShift) && 
                      _stat.TryUseStamina(_runStaminaUsage * Time.deltaTime);
        return canRun ? _runningSpeed : _movementSpeed;
    }
    
    private void Move()
    {
        float movementSpeed = GetSpeed();
        Vector3 direction = GetDirection();
        _controller.Move(direction * (movementSpeed * Time.deltaTime));
    }

    private Vector3 GetDirection()
    {        
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(xMovement, 0, zMovement);
        direction.Normalize();
        direction = _mainCamera.transform.TransformDirection(direction);
        direction.y = _yVelocity;
        return direction;
    }
    
    private void Jump()
    {
        _yVelocity += Gravity * Time.deltaTime;
        if (!Input.GetButtonDown("Jump")) return;

        if (_controller.isGrounded)
        {
            _jumpCounter = 1;
            _yVelocity = _jumpPower;
            return;
        }

        bool canAirJump = _jumpCounter < _maxJumpCount 
                          && _stat.TryUseStamina(_jumpStaminaUsage);

        if (canAirJump)
        {
            _jumpCounter++;
            _yVelocity = _jumpPower;
        }
    }
}
