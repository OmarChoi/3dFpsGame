public class ZombieHitState : ZombieStateBase, IAnimationEventHandler
{
    public ZombieHitState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        _zombie.Agent.isStopped = true;
        _zombie.Agent.ResetPath();
        
        UnityEngine.Vector3 direction = -_zombie.LastDamage.Normal;
        direction.y = 0f;
        direction.Normalize();

        Movement.ExecuteKnockback(direction, _zombie.LastDamage.Value);
    }
    
    protected override void OnExit()
    {
        Agent.isStopped = false;
    }
    
    public void OnAnimationEnd()
    {
        TransitionTo(EZombieState.Idle);
    }
}
