using UnityEngine;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerStats))]
public class PlayerMove : MonoBehaviour
{
    [System.Serializable]
    public class MoveConfig
    {
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
        if (!GameManager.Instance.CanPlay()) return;
        if (!CursorManager.Instance.IsCursorLocked) return;
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
        
        Vector3 horizontalVelocity = direction * _stats.MoveSpeed; 
        Vector3 moveVector = horizontalVelocity + (Vector3.up * _yVelocity); 
        _controller.Move(moveVector * Time.deltaTime);
    }

    private Vector3 GetDirection()
    {        
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(xMovement, 0, zMovement);
        direction.Normalize();
        direction = _mainCamera.transform.TransformDirection(direction);
        direction.y = 0f;
        direction.Normalize();

        return direction;
    }
    
    private void ApplyGravity()
    {
        if (_controller.isGrounded && _yVelocity < 0)
        {
            _yVelocity = -1f;
        }
        else
        {
            _yVelocity += Define.Gravity * Time.deltaTime;
        }
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
