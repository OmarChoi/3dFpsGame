using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    
    // 유니티는 0 ~ 360 각도 체계이므로 우리가 따로 저장할 -360 ~ 360 체계로 누적할 변수
    private float _accumulationX;
    private float _accumulationY;
    
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        _accumulationY += -mouseY * _rotationSpeed * Time.deltaTime;
        _accumulationY = Mathf.Clamp(_accumulationY, -90f, 90f);
        
        transform.eulerAngles = new Vector3(_accumulationY, _accumulationX, 0);
    }
}
