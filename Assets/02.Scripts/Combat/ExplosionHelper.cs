using UnityEngine;

public static class ExplosionHelper
{
    private const int MaxHitColliders = 64;
    private readonly static Collider[] _colliderCache = new Collider[MaxHitColliders];
    public static void ApplyExplosionDamage(ExplosionData data)
    {
        if (data.IsExploded) return;
        int hitCount = Physics.OverlapSphereNonAlloc(data.Center, data.Radius, _colliderCache, data.LayerMask);
        
        Damage damage = new Damage(data.Damage, data.ExplosionObject);
        
        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = _colliderCache[i];
            if (hit.gameObject == data.ExplosionObject.gameObject) continue;
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damageable.TryTakeDamage(damage);
            }
        }
    }
}
