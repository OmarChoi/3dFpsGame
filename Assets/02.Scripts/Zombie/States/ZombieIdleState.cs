using UnityEngine;

public class ZombieIdleState : ZombieStateBase
{
    public ZombieIdleState(Zombie zombie) : base(zombie) { }
    private float _enterIdleTime;
    
    protected override void OnEnter()
    {
        _enterIdleTime = Time.time;
        Agent.ResetPath();
    }

    protected override void OnUpdate()
    {
        if (Transform.IsInRange(Player.position, _zombie.Stats.DetectDistance))
        {
            Agent.speed = _zombie.Stats.RunSpeed;
            Animator.SetTrigger(ZombieAnimatorHash.IdleToTrace);
            TransitionTo(EZombieState.Trace);
            return;
        }

        if (Time.time - _enterIdleTime >= _zombie.Stats.WaitPatrolTime)
        {
            Agent.speed = _zombie.Stats.MoveSpeed;
            Animator.SetTrigger(ZombieAnimatorHash.IdleToPatrol);
            TransitionTo(EZombieState.Patrol);
        }
    }

    protected override void OnExit()
    {
        _enterIdleTime = 0;
    }
}
