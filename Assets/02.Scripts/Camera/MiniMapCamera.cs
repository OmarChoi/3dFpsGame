using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _yOffset = 10.0f;
    private void LateUpdate()
    {
        UpdateRotation();
        UpdatePosition();
    }

    private void UpdateRotation()
    {
        Vector3 targetAngle = _target.eulerAngles;
        transform.eulerAngles = new Vector3(90f, _target.eulerAngles.y, 0f);
    }
    
    private void UpdatePosition()
    {
        Vector3 targetPosition = _target.position;
        Vector3 finalPosition = targetPosition + new Vector3(0, _yOffset, 0);
        transform.position = finalPosition;
    }
}
