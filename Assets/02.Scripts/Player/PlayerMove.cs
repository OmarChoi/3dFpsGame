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
    private const float Gravity = -9.81f;
    private float _yVelocity = 0.0f;    // 중력에 의해 누적될 y값 변수
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _stat = GetComponent<PlayerStat>();
        _mainCamera = Camera.main;
    }
    
    private void Update()
    {
        Move();
    }

    private float GetSpeed()
    {
        float speed = _movementSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && _stat.TryUseStamina(_runStaminaUsage * Time.deltaTime))
        {
            speed = _runningSpeed;
        }
        return speed;
    }
    
    private void Move()
    {
        float movementSpeed = GetSpeed();
        float yVelocity = GetYVelocity();
        Vector3 direction = GetDirection(yVelocity);
        _controller.Move(direction * (movementSpeed * Time.deltaTime));
    }

    private Vector3 GetDirection(float yVelocity)
    {        
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(xMovement, 0, zMovement);
        direction.Normalize();
        direction = _mainCamera.transform.TransformDirection(direction);
        direction.y = yVelocity;
        return direction;
    }
    
    private float GetYVelocity()
    {
        _yVelocity += Gravity * Time.deltaTime;
        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _yVelocity = _jumpPower;
        }
        return _yVelocity;
    }
}
