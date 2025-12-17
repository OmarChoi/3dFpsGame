using UnityEngine;

public class TopViewCamera : BaseCamera
{
    [SerializeField] private float _distance = 30f;
    [SerializeField] private float _angle = 45f;
    
    public override Vector3 CalculateCameraPosition()
    {
        Vector3 offset = Quaternion.Euler(_angle, 0, 0) * (Vector3.back * _distance);
        return _target.position + offset;
    }

    public override void Rotate(float mouseX, float mouseY)
    {
        RecoverRecoil();
    }

    public override Quaternion GetRotation()
    {
        Vector3 direction = _target.position - CalculateCameraPosition();
        Quaternion baseRotation = Quaternion.LookRotation(direction);
        Quaternion recoilRotation = Quaternion.Euler(_recoilY, _recoilX, 0f);
    
        return recoilRotation * baseRotation;
    }
}
