using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HitEffectUI : MonoBehaviour
{
    [SerializeField] private Image _hitEffect;
    [SerializeField] private Sprite[] _imageSources;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private float _effectDuration;
    private Tweener _effectTween;
    private Color _fullAlphaColor;

    private void Start()
    { 
        _fullAlphaColor = _hitEffect.color;
        Color color = _fullAlphaColor;
        color.a = 0;
        _hitEffect.color = color;
    }
    
    private void OnEnable()
    {
        _playerHealth.OnHit += PlayHitEffect;
    }

    private void OnDisable()
    {
        _playerHealth.OnHit -= PlayHitEffect;
    }
    
    private void PlayHitEffect()
    {
        SetImage();
        ResetAlpha();
        AnimateEffect();
    }

    private void SetImage()
    {
        int randomIndex = UnityEngine.Random.Range(0, _imageSources.Length);
        _hitEffect.sprite = _imageSources[randomIndex];
    }
    
    private void ResetAlpha()
    {
        Color color = _hitEffect.color;
        color.a = 1f;
        _hitEffect.color = color;
    }

    private void AnimateEffect()
    {
        _effectTween?.Kill();
        _hitEffect.DOFade(0, _effectDuration).SetEase(Ease.InQuad);
    }
}
