using System;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public static event Action ThrowAnimationEnd;

    public void ThrowAnimationEndEvent()
    {
        ThrowAnimationEnd?.Invoke();
    }
}
