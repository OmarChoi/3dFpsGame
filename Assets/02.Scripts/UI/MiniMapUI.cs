using UnityEngine;
using DG.Tweening;

public class MiniMapUI : MonoBehaviour
{
    [SerializeField] Camera _miniMapCamera;

    [Header("Zoom")]
    [Space]
    [SerializeField] private int[] _zoomLevels;
    [SerializeField] private float _zoomSpeed;
    private int _currentLevel = 1;
    private Tweener _zoomTween;

    private void Start()
    {
        if (_zoomLevels.Length == 0) return;
        _miniMapCamera.orthographicSize = _zoomLevels[_currentLevel];
    }
    
    public void ZoomIn()
    {
        _currentLevel = Mathf.Clamp(_currentLevel - 1, 0, _zoomLevels.Length - 1);
        AnimateZoom(_zoomLevels[_currentLevel]);
    }

    public void ZoomOut()
    {
        _currentLevel = Mathf.Clamp(_currentLevel + 1, 0, _zoomLevels.Length - 1);
        AnimateZoom(_zoomLevels[_currentLevel]);
    }
    
    private void AnimateZoom(float targetSize)
    {
        _zoomTween?.Kill();
        _zoomTween = _miniMapCamera.DOOrthoSize(targetSize, _zoomSpeed)
                                   .SetEase(Ease.OutSine);
    }

    private void OnDestroy()
    {
        _zoomTween?.Kill();
    }
}
