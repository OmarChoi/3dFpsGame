using UnityEngine;

public abstract class BaseCamera : MonoBehaviour
{
    [SerializeField] protected Transform _target;
    [SerializeField] private float _yMinLimit;
    [SerializeField] private float _yMaxLimit;
    [SerializeField] private float _rotationSpeed;
    
    private float _accumulationX;
    private float _accumulationY;
    
    public abstract Vector3 CalculateCameraPosition();
    
    protected float _recoilX;
    protected float _recoilY;
    [SerializeField] private float _recoilRecoverySpeed;
    
    public virtual void Rotate(float mouseX, float mouseY)
    {
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        _accumulationY -= mouseY * _rotationSpeed * Time.deltaTime;
        _accumulationY = Mathf.Clamp(_accumulationY, _yMinLimit, _yMaxLimit);
        RecoverRecoil();
    }

    public virtual Quaternion GetRotation()
    {
        float rotationY = Mathf.Clamp(_accumulationY + _recoilY, _yMinLimit, _yMaxLimit);
        return Quaternion.Euler(rotationY, _accumulationX + _recoilX, 0);
    }
    
    public void AddRecoil(float verticalRecoil, float horizontalRecoil)
    {
        _recoilY -= verticalRecoil;
        _recoilX += horizontalRecoil;
    }
    
    protected void RecoverRecoil()
    {
        float recoverySpeed = _recoilRecoverySpeed * Time.deltaTime;
        _recoilX = Mathf.Lerp(_recoilX, 0f, recoverySpeed);
        _recoilY = Mathf.Lerp(_recoilY, 0f, recoverySpeed);
    }
    
    public void Init(float x, float y)
    {
        _accumulationX = x;
        _accumulationY = y;
    }

    public (float, float) GetAngle()
    {
        return (_accumulationX + _recoilX, _accumulationY + _recoilY);
    }
}