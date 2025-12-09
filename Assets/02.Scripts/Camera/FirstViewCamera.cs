using UnityEngine;

public class FirstViewCamera : BaseCamera
{
    public override Vector3 CalculateCameraPosition()
    {
        return _target.position;
    }
    
    public override void Move()
    {
        this.transform.position = _target.position;
    }
    
    public override void Rotate(float mouseX, float mouseY)
    {
        base.Rotate(mouseX, mouseY);
        transform.eulerAngles = new Vector3(_accumulationY, _accumulationX, 0);
    }
}
