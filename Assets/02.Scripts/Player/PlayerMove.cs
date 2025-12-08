using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 7.0f;
    
    private void Update()
    {
        float xMovement = Input.GetAxis("Horizontal");
        float zMovement = Input.GetAxis("Vertical");
        
        Vector3 direction = new Vector3(xMovement, 0, zMovement);
        direction.Normalize();
        direction = Camera.main.transform.TransformDirection(direction);
        
        transform.position += direction * (_movementSpeed * Time.deltaTime);
    }
}
