using UnityEngine;

public class ZombieHitState : ZombieDataStateBase<Damage>, IAnimationEventHandler
{
    public ZombieHitState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter(in Damage damage)
    {
        Vector3 direction = damage.Normal;
        direction.y = 0f;
        direction.Normalize();

        Movement.ExecuteKnockback(direction, damage.Value, OnKnockbackComplete);
    }

    protected override void OnUpdate()
    {
        // Knockback execution is handled by ZombieMovement.ExecuteKnockback
    }

    protected override void OnExit()
    {
        Agent.isStopped = false;
    }

    private void OnKnockbackComplete()
    {
        // Knockback finished, waiting for animation end
    }

    public void OnAnimationEnd()
    {
        TransitionTo(EZombieState.Idle);
    }
}
