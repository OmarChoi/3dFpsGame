using System;
using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStats _stats;
    public event Action OnHit;
    private Animator _animator;

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
        _animator = GetComponentInChildren<Animator>();
        _stats.Health.OnValueChanged += SetInjuryAnimation;
    }

    private void SetInjuryAnimation(float health, float maxHealth)
    {
        _animator.SetLayerWeight(2, (1.0f - (_stats.Health.Value / _stats.Health.MaxValue)));
    }
    
    public bool TryTakeDamage(in Damage damage)
    {
        if (_stats.Health.Value <= 0) return false;
        _stats.Health.Consume(damage.Value);
        OnHit?.Invoke();
        if (_stats.Health.Value <= 0)
        {
            // Todo. 플레이어 사망 처리
            GameManager.Instance.GameOver();
        }
        return true;
    }
}
