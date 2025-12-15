using UnityEngine;

[RequireComponent(typeof(PlayerStats))]
public class PlayerHealth : MonoBehaviour, IDamageable
{
    private PlayerStats _stats;

    private void Awake()
    {
        _stats = GetComponent<PlayerStats>();
    }
    
    public bool TryTakeDamage(in Damage damage)
    {
        if (_stats.Health.Value <= 0) return false;
        _stats.Health.Decrease(damage.Value);
        if (_stats.Health.Value <= 0)
        {
            // Todo. 플레이어 사망 처리
            GameManager.Instance.GameOver();
        }
        return true;
    }
}
