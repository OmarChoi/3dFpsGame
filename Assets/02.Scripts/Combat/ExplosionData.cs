using System;
using UnityEngine;

[Serializable]
public struct ExplosionData
{
    #region Object
    public GameObject     ExplosionObject;
    public ParticleSystem Effect;
    #endregion

    #region Overlap Area
    public Vector3 Center;
    public float Radius;
    #endregion
    
    #region Damageable
    public float Damage;
    public LayerMask LayerMask;
    #endregion
    
    public float ExplosionForce;
    public bool IsExploded;
}