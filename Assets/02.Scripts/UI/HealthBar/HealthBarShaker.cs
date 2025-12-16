using DG.Tweening;
using UnityEngine;

public class HealthBarShaker : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _healthBarTransform;
    
    [Header("Shake Settings - Damage")]
    [SerializeField] private float _strength = 8f;
    [SerializeField] private float _duration = 0.4f;
    
    [Header("Advanced Settings")]
    [SerializeField] private int _shakeVibrato = 15;
    [SerializeField] private float _shakeRandomness = 90f;
    
    private Sequence _shakeSequence;
    
    private void OnDestroy()
    {
        CleanupTweens();
    }
    
    public void ShakeOnDamage()
    {
        Shake(_strength, _duration);
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
                vibrato: _shakeVibrato,
                randomness: _shakeRandomness,
                fadeOut: true
            ))
            .OnComplete(() => _healthBarTransform.anchoredPosition = Vector2.zero);
    }
    
    private void CleanupTweens()
    {
        _shakeSequence?.Kill();
    }
}
