using UnityEngine;

public static class Util
{
    public static float GetSquaredDistance(this Transform self, Vector3 targetPosition)
    {
        return (self.position - targetPosition).sqrMagnitude;
    }

    public static bool IsInRange(this Transform self, Vector3 target, float range)
    {
        return self.GetSquaredDistance(target) <= range * range;
    }
    
    public static bool IsInRange(float squaredDistance, float range)
    {
        return squaredDistance <= range * range;
    }
}
