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
    
    
    private void Awake()
    {
        _zombie = GetComponent<Zombie>();
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        float currentHealth = _zombie.Health.Value / _zombie.Health.MaxValue;
        if (!Mathf.Approximately(_prevHealth, currentHealth))
        {
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
}
