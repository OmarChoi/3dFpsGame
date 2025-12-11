using UnityEngine;

public struct Damage
{
    public float Value;
    public GameObject Attacker;

    public Damage(float damage, GameObject attacker)
    {
        Value = damage;
        Attacker = attacker;
    }
}
