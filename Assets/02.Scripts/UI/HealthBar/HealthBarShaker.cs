using DG.Tweening;
using UnityEngine;

public class HealthBarShaker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _healthBarTransform;
    [SerializeField] private ShakeInfo _shakeInfo;
    private Sequence _shakeSequence;
    
    private void OnDestroy()
    {
        CleanupTweens();
    }
    
    public void ShakeOnDamage()
    {
        Shake(_shakeInfo.Strength, _shakeInfo.Duration);
    }
    
    private void Shake(float strength, float duration)
    {
        if (_healthBarTransform == null) return;
        
        _shakeSequence?.Kill();
        _healthBarTransform.anchoredPosition = Vector2.zero;
        
        _shakeSequence = DOTween.Sequence()
            .Append(_healthBarTransform.DOShakePosition(
                duration: duration,
                strength: strength,
                vibrato: _shakeInfo.Vibrato,
                randomness: _shakeInfo.Randomness,
                fadeOut: true
            ))
            .OnComplete(() => _healthBarTransform.anchoredPosition = Vector2.zero);
    }
    
    private void CleanupTweens()
    {
        _shakeSequence?.Kill();
    }
}
