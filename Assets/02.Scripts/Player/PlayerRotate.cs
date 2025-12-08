using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    private float _accumulationX;
    
    private void Update()
    {
        if (!Input.GetMouseButton(1)) return;
        float mouseX = Input.GetAxis("Mouse X");
        
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        
        transform.eulerAngles = new Vector3(0, _accumulationX, 0);
    }
}
