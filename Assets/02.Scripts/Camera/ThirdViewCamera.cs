using UnityEngine;

public class ThirdViewCamera : BaseCamera
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _distance;

    public override Vector3 CalculateCameraPosition()
    {
        Quaternion currentRotation = Quaternion.Euler(_accumulationY, _accumulationX, 0);
        Vector3 direction = currentRotation * Vector3.back;
        Vector3 cameraPosition = _target.position + direction * _distance + new Vector3(0, _offset.y, 0);
        return cameraPosition;
    }
    
    public override void Move()
    {
        Vector3 cameraPosition = CalculateCameraPosition();
        transform.position = cameraPosition;
        transform.LookAt(_target.position);
    }
}
