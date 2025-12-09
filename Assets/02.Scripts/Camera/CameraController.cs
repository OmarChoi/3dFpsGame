using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum ECameraType
    {
        FirstView, 
        ThirdView
    }

    [SerializeField] private float _cameraChangeDuration;

    private BaseCamera[] _cameras;
    private ECameraType _currentType = ECameraType.FirstView;
    private ECameraType _previousType;

    private bool _isSwitching;
    private float _blend;

    private void Awake()
    {
        _cameras = GetComponentsInChildren<BaseCamera>();
    }

    private void Update()
    {
        GetKeyInput();
    }

    private void LateUpdate()
    {
        if (_isSwitching)
        {
            ProcessCameraTransition();
        }
        else
        {
            FollowActiveCamera();
        }
    }

    private void GetKeyInput()
    {
        if (!_isSwitching && Input.GetKeyDown(KeyCode.T))
        {
            StartChangingCamera();
        }
    }

    private void StartChangingCamera()
    {
        BaseCamera prevCamera = _cameras[(int)_currentType];
        (float x, float y) = prevCamera.GetAngle();

        _previousType = _currentType;
        _currentType = (_currentType == ECameraType.FirstView) ? ECameraType.ThirdView : ECameraType.FirstView;

        BaseCamera nextCamera = _cameras[(int)_currentType];
        nextCamera.Init(x, y);

        _blend = 0f;
        _isSwitching = true;

        DOVirtual.Float(0f, 1f, _cameraChangeDuration, value => _blend = value)
            .SetEase(Ease.OutSine)
            .OnComplete(() => _isSwitching = false);
    }

    private void ProcessCameraTransition()
    {
        BaseCamera prevCamera = _cameras[(int)_previousType];
        BaseCamera nextCamera = _cameras[(int)_currentType];

        ApplyRotationInput(prevCamera);
        ApplyRotationInput(nextCamera);
        
        Vector3 prevPosition = prevCamera.CalculateCameraPosition();
        Vector3 nextPosition = nextCamera.CalculateCameraPosition();
        Vector3 blendedPos = Vector3.Lerp(prevPosition,  nextPosition, _blend);

        Quaternion prevRotation = prevCamera.GetRotation();
        Quaternion nextRotation = nextCamera.GetRotation();
        Quaternion blendedRot = Quaternion.Slerp(prevRotation, nextRotation, _blend);

        ApplyToTransform(blendedPos, blendedRot);
    }

    private void FollowActiveCamera()
    {
        BaseCamera active = _cameras[(int)_currentType];
        ApplyRotationInput(active);
        active.Move();
        ApplyToTransform(active.CalculateCameraPosition(), active.GetRotation());
    }

    private void ApplyRotationInput(BaseCamera cam)
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        cam.Rotate(mouseX, mouseY);
    }

    private void ApplyToTransform(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }
}
