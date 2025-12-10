using UnityEngine;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerStats))]
public class PlayerMove : MonoBehaviour
{
    [System.Serializable]
    public class MoveConfig
    {
        public float Gravity;
        public float RunStaminaUsage;
        public float JumpStaminaUsage;
    }

    private CharacterController _controller;
    private PlayerStats _stats;
    private Camera _mainCamera;
    [SerializeField] private MoveConfig _config;
    
    [SerializeField] private float _maxJumpCount;
    private float _yVelocity;
    private int _jumpCounter;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stats = GetComponent<PlayerStats>();
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        ApplyGravity();
        HandleJump();
        Move();
    }

    private float GetSpeed()
    {
        bool canRun = Input.GetKey(KeyCode.LeftShift) && 
                      _stats.Stamina.TryConsume(_config.RunStaminaUsage * Time.deltaTime);
        return canRun ? _stats.RunSpeed : _stats.MoveSpeed;
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
    
    private void ApplyGravity()
    {
        _yVelocity += _config.Gravity * Time.deltaTime;
    }

    private void HandleJump()
    {
        if (!Input.GetButtonDown("Jump")) return;

        if (_controller.isGrounded)
        {
            _jumpCounter = 1;
            _yVelocity = _stats.JumpPower;
            return;
        }

        bool canAirJump = _jumpCounter < _maxJumpCount 
                          && _stats.Stamina.TryConsume(_config.JumpStaminaUsage);

        if (canAirJump)
        {
            _jumpCounter++;
            _yVelocity = _stats.JumpPower;
        }
    }
}
