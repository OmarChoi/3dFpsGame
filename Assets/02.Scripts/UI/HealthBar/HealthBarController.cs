using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private HealthBarAnimator _animator;
    [SerializeField] private HealthBarShaker _shaker;
    
    public void UpdateHealth(float health, float maxHealth)
    {
        if (maxHealth <= 0) return;
        
        float targetValue = health / maxHealth;
        float currentValue = _animator.GetCurrentHealthValue();
        
        if (targetValue < currentValue)
        {
            HandleHealthDecrease(targetValue);
        }
        else if (targetValue > currentValue)
        {
            HandleHealthIncrease(targetValue);
        }
    }
    
    private void HandleHealthDecrease(float targetValue)
    {
        _animator.AnimateHealthDecrease(targetValue);
        _shaker.ShakeOnDamage();
    }
    
    private void HandleHealthIncrease(float targetValue)
    {
        _animator.AnimateHealthIncrease(targetValue);
    }
}
