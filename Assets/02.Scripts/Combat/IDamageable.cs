using UnityEngine;

public interface IDamageable
{
    public bool TryTakeDamage(in Damage damage);
}
