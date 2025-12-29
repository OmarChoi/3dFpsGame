using UnityEngine;

public static class ZombieAnimatorHash
{
    // Event Triggers
    public static readonly int Death = Animator.StringToHash("Death");
    public static readonly int Hit = Animator.StringToHash("Hit");
    public static readonly int Attack = Animator.StringToHash("Attack");

    // Transition Triggers
    public static readonly int AttackToTrace = Animator.StringToHash("AttackToTrace");
    public static readonly int IdleToTrace = Animator.StringToHash("IdleToTrace");
    public static readonly int IdleToPatrol = Animator.StringToHash("IdleToPatrol");
    public static readonly int TraceToAttack = Animator.StringToHash("TraceToAttack");
    public static readonly int TraceToComeback = Animator.StringToHash("TraceToComeback");
    public static readonly int TraceToJump = Animator.StringToHash("TraceToJump");
    public static readonly int JumpToTrace = Animator.StringToHash("JumpToTrace");
    public static readonly int PatrolToTrace = Animator.StringToHash("PatrolToTrace");
    public static readonly int ComebackToTrace = Animator.StringToHash("ComebackToTrace");
    public static readonly int ComebackToIdle = Animator.StringToHash("ComebackToIdle");
}
