using UnityEngine;

public class ThirdViewCamera : BaseCamera
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float   _distance;

    public override Vector3 CalculateCameraPosition()
    {
        Quaternion rot = GetRotation();
        Vector3 direction = rot * Vector3.back;
        return _target.position + direction * _distance + _offset;
    }

    public override void Move()
    {
        transform.position = CalculateCameraPosition();
    }
}