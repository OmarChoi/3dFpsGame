using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarAnimator : MonoBehaviour
{
    [SerializeField] private Slider _healthBarSlider;
    [SerializeField] private Image _delayedHealthBarImage;
    [SerializeField] private DelayedInfo _delayedInfo;

    private Tween _delayedHealthTween;
    
    private enum HealthBarType
    {
        Slider,
        Image
    }
    
    private void Start()
    {
        InitializeHealthBar();
    }
    
    private void OnDestroy()
    {
        CleanupTweens();
    }
    
    private void InitializeHealthBar()
    {
        _delayedHealthBarImage.fillAmount = _healthBarSlider.value;
    }
    
    public void AnimateHealthDecrease(float targetValue)
    {
        float currentValue = GetCurrentHealthValue();
        SetHealthBarValue(targetValue);
        AnimateDelayedHealthBar(currentValue, targetValue);
    }
    
    public void AnimateHealthIncrease(float targetValue)
    {
        SetHealthBarValue(targetValue);
    }
    
    private void AnimateDelayedHealthBar(float currentValue, float targetValue)
    {
        if (_delayedHealthTween != null && _delayedHealthTween.IsActive())
        {
            _delayedHealthTween.Kill();
        }
        else
        {
            SetDelayedHealthBarValue(currentValue);
        }

        _delayedHealthTween = 
            DOTween.To(() => _delayedHealthBarImage.fillAmount,
                       x => _delayedHealthBarImage.fillAmount = x, 
                       targetValue,
                       _delayedInfo.Duration).
                    SetDelay(_delayedInfo.StartDelay)
                    .SetEase(_delayedInfo.DelayedBarEase);
    }
    
    public float GetCurrentHealthValue()
    {
        return _healthBarSlider != null ? _healthBarSlider.value : 0f;
    }
    
    private void SetHealthBarValue(float value)
    {
        _healthBarSlider.value = value;
    }
    
    private void SetDelayedHealthBarValue(float value)
    {
        _delayedHealthBarImage.fillAmount = value;
    }
    
    private void CleanupTweens()
    {
        _delayedHealthTween?.Kill();
    }
}
