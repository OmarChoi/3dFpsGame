using System;
using UnityEngine;

[Serializable]
public struct ExplosionData
{
    public ParticleSystem Effect;
    public bool IsExploded;
    public float Damage;
    public float ExplosionForce ;
    public float Radius;
    public LayerMask LayerMask;
}