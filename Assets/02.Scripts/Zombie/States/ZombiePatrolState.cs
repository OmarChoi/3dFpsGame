using UnityEngine;

public class ZombiePatrolState : ZombieStateBase
{
    private Vector3 _patrolDestination;

    public ZombiePatrolState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        Agent.ResetPath();
        Agent.speed = _zombie.Stats.MoveSpeed;
        _patrolDestination = Movement.GetRandomPositionInRange(_zombie.StartPosition, _zombie.Stats.PatrolDistance);
    }

    protected override void OnUpdate()
    {
        Movement.Move(_patrolDestination);

        if (Transform.IsInRange(Player.position, _zombie.Stats.DetectDistance))
        {
            Agent.ResetPath();
            Agent.speed = _zombie.Stats.RunSpeed;
            Animator.SetTrigger(ZombieAnimatorHash.PatrolToTrace);
            TransitionTo(EZombieState.Trace);
            return;
        }

        if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            Agent.ResetPath();
            Agent.speed = _zombie.Stats.MoveSpeed;
            _patrolDestination = Movement.GetRandomPositionInRange(_zombie.StartPosition, _zombie.Stats.PatrolDistance);
        }
    }
}
