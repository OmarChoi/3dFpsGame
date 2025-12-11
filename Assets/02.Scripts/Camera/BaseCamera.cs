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
    private float _recoilApplySpeed = 0.2f;
    
    public virtual void Rotate(float mouseX, float mouseY)
    {
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        _accumulationY -= mouseY * _rotationSpeed * Time.deltaTime;
        _accumulationY = Mathf.Clamp(_accumulationY, _yMinLimit, _yMaxLimit);
    }

    public virtual Quaternion GetRotation()
    {
        return Quaternion.Euler(_accumulationY, _accumulationX, 0);
    }
    
    public void AddRecoil(float verticalRecoil, float horizontalRecoil)
    {
        _accumulationY -= verticalRecoil;
        _accumulationX += horizontalRecoil;
        _accumulationY = Mathf.Clamp(_accumulationY, _yMinLimit, _yMaxLimit);
    }
    
    public void Init(float x, float y)
    {
        _accumulationX = x;
        _accumulationY = y;
    }

    public (float, float) GetAngle()
    {
        return (_accumulationX, _accumulationY);
    }
}