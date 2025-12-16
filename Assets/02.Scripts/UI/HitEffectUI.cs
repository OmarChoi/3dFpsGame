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

    private void OnDestroy()
    {
        ClearTweens();
    }
    
    private void PlayHitEffect()
    {
        SetImage();
        ResetAlpha();
        _effectTween?.Kill();
        AnimateEffect();
    }

    private void SetImage()
    {
        if (_imageSources == null || _imageSources.Length == 0) return;
        int randomIndex = UnityEngine.Random.Range(0, _imageSources.Length);
        _hitEffect.sprite = _imageSources[randomIndex];
    }
    
    private void ResetAlpha()
    {
        _hitEffect.color = _fullAlphaColor;
    }

    private void AnimateEffect()
    {
        _effectTween = _hitEffect.DOFade(0, _effectDuration).SetEase(Ease.InQuad);
    }

    private void ClearTweens()
    {
        _effectTween?.Kill();
    }
}
