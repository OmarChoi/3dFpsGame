using UnityEngine;

public class FirstViewCamera : BaseCamera
{
    public override Vector3 CalculateCameraPosition()
    {
        return _target.position;
    }

    public override void Move()
    {
        transform.position = _target.position;
    }
}