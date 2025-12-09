using UnityEngine;

public abstract class BaseCamera : MonoBehaviour
{
    [SerializeField] protected Transform _target;
    [SerializeField] private float _yMinLimit;
    [SerializeField] private float _yMaxLimit;
    [SerializeField] private float _rotationSpeed;
    
    protected float _accumulationX;
    protected float _accumulationY;
    
    public abstract Vector3 CalculateCameraPosition();
    
    public abstract void Move();
    
    public virtual void Rotate(float mouseX, float mouseY)
    {
        _accumulationX += mouseX * _rotationSpeed * Time.deltaTime;
        _accumulationY += -mouseY * _rotationSpeed * Time.deltaTime;
        _accumulationY = Mathf.Clamp(_accumulationY, _yMinLimit, _yMaxLimit);
    }
    
    public void Init(float mouseX, float mouseY)
    {
        _accumulationX = mouseX;
        _accumulationY = mouseY;
    }

    public (float, float) GetAngle()
    {
        return (_accumulationX, _accumulationY);
    }
}
