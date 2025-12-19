using UnityEngine;

public static class ExplosionHelper
{
    private const int MaxHitColliders = 64;
    private readonly static Collider[] _colliderCache = new Collider[MaxHitColliders];
    public static void ApplyExplosionDamage(in ExplosionData data)
    {
        int hitCount = Physics.OverlapSphereNonAlloc(data.Center, data.Radius, _colliderCache, data.LayerMask);

        Damage damage = new Damage()
        {
            Value = data.Damage,
            Attacker = data.ExplosionObject,
            Critical = false,
        };
        
        for (int i = 0; i < hitCount; i++)
        {
            Collider hit = _colliderCache[i];
            if (hit.gameObject == data.ExplosionObject.gameObject) continue;
            if (hit.TryGetComponent(out IDamageable damageable))
            {
                damage.HitPosition = hit.ClosestPoint(data.Center);
                damage.Normal = (damage.HitPosition - data.Center).normalized;
                damageable.TryTakeDamage(damage);
            }
        }
    }
}
