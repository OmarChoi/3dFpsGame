using UnityEngine;

public class FirstViewCamera : BaseCamera
{
    public override Vector3 CalculateCameraPosition()
    {
        return _target.position;
    }
}