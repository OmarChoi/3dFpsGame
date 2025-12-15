using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private float _accumulationX;
    
    private void Update()
    {
        if (GameManager.Instance.State != EGameState.Playing) return;
        
        float mouseX = Input.GetAxis("Mouse X");
        
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        
        transform.rotation = Quaternion.Euler(0, _accumulationX, 0);
    }
}
