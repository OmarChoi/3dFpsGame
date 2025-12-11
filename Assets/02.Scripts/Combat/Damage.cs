using UnityEngine;

public struct Damage
{
    public float Value;
    public Vector3 Attacker;

    public Damage(float damage, Vector3 attacker)
    {
        Value = damage;
        Attacker = attacker;
    }
}
