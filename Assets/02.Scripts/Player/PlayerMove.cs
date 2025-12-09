using UnityEngine;

[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(PlayerStat))]
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
    private PlayerStat _stat;
    private Camera _mainCamera;
    [SerializeField] private MoveConfig _config;
    
    [SerializeField] private float _maxJumpCount;
    private float _yVelocity;
    private int _jumpCounter;
    
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
                      _stat.Stamina.TryConsume(_config.RunStaminaUsage * Time.deltaTime);
        return canRun ? _stat.RunSpeed : _stat.MoveSpeed;
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
        _yVelocity += _config.Gravity * Time.deltaTime;
        if (!Input.GetButtonDown("Jump")) return;

        if (_controller.isGrounded)
        {
            _jumpCounter = 1;
            _yVelocity = _stat.JumpPower;
            return;
        }

        bool canAirJump = _jumpCounter < _maxJumpCount 
                          && _stat.Stamina.TryConsume(_config.JumpStaminaUsage);

        if (canAirJump)
        {
            _jumpCounter++;
            _yVelocity = _stat.JumpPower;
        }
    }
}
