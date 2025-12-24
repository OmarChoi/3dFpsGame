using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Zombie))]
public class ZombieHealthBar : MonoBehaviour
{
    private Zombie _zombie;
    [SerializeField] private Image _gaugeImage;
    [SerializeField] private Transform _healthBarTransform;
    private Camera _mainCamera;
    private float _prevHealth = -1;
    
    [Header("딜레이 세팅")]
    [Space]
    [SerializeField] private Image _delayedHealthImage;
    [SerializeField] private DelayedInfo _delayedInfo;
    private Tween _delayedHealthTween;
    
    [Header("Shake 세팅")]
    [Space]
    [SerializeField] private ShakeInfo _shakeInfo;
    private RectTransform _rectTransform;
    private Sequence  _shakeSequence;
    
    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _mainCamera = Camera.main;
        _rectTransform = _healthBarTransform.GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        float currentHealth = _zombie.Health.Value / _zombie.Health.MaxValue;
        if (!Mathf.Approximately(_prevHealth, currentHealth))
        {
            Shake(_shakeInfo.Strength, _shakeInfo.Duration);
            _gaugeImage.fillAmount = currentHealth;
            AnimateDelayedHealthBar(_prevHealth, currentHealth);
            _prevHealth = currentHealth;
        }
        
        _healthBarTransform.forward = _mainCamera.transform.forward;
    }

    private void AnimateDelayedHealthBar(float currentValue, float targetValue)
    {
        if (_delayedHealthTween != null && _delayedHealthTween.IsActive())
        {
            _delayedHealthTween.Kill();
        }
        else
        {
            _delayedHealthImage.fillAmount = currentValue;
        }

        _delayedHealthTween = 
            DOTween.To(() => _delayedHealthImage.fillAmount,
                       x => _delayedHealthImage.fillAmount = x, 
                       targetValue,
                       _delayedInfo.Duration).
                    SetDelay(_delayedInfo.StartDelay)
                    .SetEase(_delayedInfo.DelayedBarEase);
    }
    
    private void Shake(float strength, float duration)
    {
        _shakeSequence?.Kill();
        _rectTransform.anchoredPosition = Vector2.zero;
        
        _shakeSequence = DOTween.Sequence()
            .Append(_healthBarTransform.DOShakePosition(
                duration: duration,
                strength: strength,
                vibrato: _shakeInfo.Vibrato,
                randomness: _shakeInfo.Randomness,
                fadeOut: true
            ))
            .OnComplete(() => _rectTransform.anchoredPosition = Vector2.zero);
    }

    private void OnDestroy()
    {
        ClearTweens();
    }

    private void ClearTweens()
    {
        _shakeSequence?.Kill();
        _delayedHealthTween?.Kill();
    }
}
