using UnityEngine;

public class ZombiePatrolState : ZombieStateBase
{
    private Vector3 _patrolDestination;

    public ZombiePatrolState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        _zombie.StopAgent();
        _zombie.Agent.speed = _zombie.Stats.MoveSpeed;
        _patrolDestination = Movement.GetRandomPositionInRange(_zombie.StartPosition, _zombie.Stats.PatrolDistance);
        Movement.MoveTo(_patrolDestination);
    }

    protected override void OnUpdate()
    {
        if (Transform.IsInRange(Player.position, _zombie.Stats.DetectDistance))
        {
            Animator.SetTrigger(ZombieAnimatorHash.PatrolToTrace);
            TransitionTo(EZombieState.Trace);
            return;
        }

        if (!Agent.pathPending && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            _patrolDestination = Movement.GetRandomPositionInRange(_zombie.StartPosition, _zombie.Stats.PatrolDistance);
            Movement.MoveTo(_patrolDestination);
        }
    }
}
