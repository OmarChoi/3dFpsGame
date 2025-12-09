using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private enum ECameraType
    {
        FirstView,
        ThirdView,
    }
    private BaseCamera[] _cameras;
    private ECameraType _currentType = ECameraType.FirstView;

    [SerializeField] private float _cameraChangeDuration;
    private float _cameraChangeTimer;
    
    private void Awake()
    {
        _cameras = GetComponentsInChildren<BaseCamera>();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeCameraView();
        }
    }

    private void LateUpdate()
    {
        if (_cameraChangeTimer > 0)
        {
            _cameraChangeTimer -= Time.deltaTime;
            return;
        }
        _cameras[(int)_currentType].Move();
        Rotate();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        _cameras[(int)_currentType].Rotate(mouseX, mouseY);
    }

    private void ChangeCameraView()
    {
        BaseCamera prevCamera = _cameras[(int)_currentType];
        (float x, float y) = prevCamera.GetAngle();
        _currentType = _currentType == ECameraType.FirstView ? ECameraType.ThirdView : ECameraType.FirstView;
        
        BaseCamera nextCamera = _cameras[(int)_currentType];
        Vector3 cameraPosition = nextCamera.CalculateCameraPosition();
        nextCamera.Init(x, y);
        
        transform.DOMove(cameraPosition, _cameraChangeDuration);
        
        _cameraChangeTimer = _cameraChangeDuration;
    }
}
