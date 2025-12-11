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
    public BaseCamera CurrentCamera => _cameras[(int)_currentType];

    private bool _isSwitching;
    private bool _isShot;
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
        if (_isShot == true) return;
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
        BaseCamera prevCamera = CurrentCamera;
        (float x, float y) = prevCamera.GetAngle();

        _previousType = _currentType;
        _currentType = (_currentType == ECameraType.FirstView) ? ECameraType.ThirdView : ECameraType.FirstView;

        BaseCamera nextCamera = CurrentCamera;
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
        BaseCamera nextCamera = CurrentCamera;

        ApplyRotationInput(prevCamera);
        ApplyRotationInput(nextCamera);
        
        Vector3 prevPosition = prevCamera.CalculateCameraPosition();
        Vector3 nextPosition = nextCamera.CalculateCameraPosition();
        Vector3 blendedPosition = Vector3.Lerp(prevPosition,  nextPosition, _blend);

        Quaternion prevRotation = prevCamera.GetRotation();
        Quaternion nextRotation = nextCamera.GetRotation();
        Quaternion blendedRotation = Quaternion.Slerp(prevRotation, nextRotation, _blend);

        ApplyToTransform(blendedPosition, blendedRotation);
    }

    private void FollowActiveCamera()
    {
        BaseCamera activeCamera = CurrentCamera;
        ApplyRotationInput(activeCamera);
        activeCamera.Move();
        ApplyToTransform(activeCamera.CalculateCameraPosition(), activeCamera.GetRotation());
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
