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
    public abstract void Move();
    
    private float _recoilX;
    private float _recoilY;
    
    public virtual void Rotate(float mouseX, float mouseY)
    {
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        _accumulationY -= mouseY * _rotationSpeed * Time.deltaTime;
        _accumulationY = Mathf.Clamp(_accumulationY, _yMinLimit, _yMaxLimit);
    
        _recoilX = Mathf.Lerp(_recoilX, 0f, Time.deltaTime);
        _recoilY = Mathf.Lerp(_recoilY, 0f, Time.deltaTime);
    }

    public virtual Quaternion GetRotation()
    {
        return Quaternion.Euler(_accumulationY + _recoilY, _accumulationX + _recoilX, 0);
    }
    
    public void AddRecoil(float verticalRecoil, float horizontalRecoil)
    {
        _recoilY -= verticalRecoil;
        _recoilX += horizontalRecoil;
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