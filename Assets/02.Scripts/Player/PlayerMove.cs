using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMove : MonoBehaviour
{
    private CharacterController _controller;
    
    [SerializeField] private float _movementSpeed = 7.0f;
    
    private readonly float _gravity = -9.81f;
    private float _yVelocity = 0.0f;    // 중력에 의해 누적될 y값 변수
    [SerializeField] private float _jumpPower = 5.0f;
    
    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }
    
    private void Update()
    {
        _yVelocity += _gravity * Time.deltaTime;
        
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(xMovement, 0, zMovement);
        direction.Normalize();

        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            _yVelocity = _jumpPower;
        }
        
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = _yVelocity;
        
        _controller.Move(direction * (_movementSpeed * Time.deltaTime));
    }
}
