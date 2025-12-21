using UnityEngine;

public class ZombieComebackState : ZombieStateBase
{
    public ZombieComebackState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        Agent.ResetPath();
        Agent.speed = _zombie.Stats.MoveSpeed;
    }

    protected override void OnUpdate()
    {
        Movement.Move(_zombie.StartPosition);

        if (Transform.IsInRange(Player.position, _zombie.Stats.DetectDistance))
        {
            Agent.ResetPath();
            Agent.speed = _zombie.Stats.RunSpeed;
            Animator.SetTrigger(ZombieAnimatorHash.ComebackToTrace);
            TransitionTo(EZombieState.Trace);
            return;
        }

        if (Transform.IsInRange(_zombie.StartPosition, _zombie.Stats.ArrivalThreshold))
        {
            Agent.ResetPath();
            Animator.SetTrigger(ZombieAnimatorHash.ComebackToIdle);
            TransitionTo(EZombieState.Idle);
        }
    }
}
