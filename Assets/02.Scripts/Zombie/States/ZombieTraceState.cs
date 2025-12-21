using UnityEngine.AI;

public class ZombieTraceState : ZombieStateBase
{
    public ZombieTraceState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        Agent.speed = _zombie.Stats.RunSpeed;
    }

    protected override void OnUpdate()
    {
        Movement.Move(Player.position);
        float distance = Transform.GetSquaredDistance(Player.position);

        if (Util.IsInRange(distance, _zombie.Stats.AttackDistance))
        {
            Agent.ResetPath();
            Animator.SetTrigger(ZombieAnimatorHash.TraceToAttack);
            TransitionTo(EZombieState.Attack);
        }
        else if (!Util.IsInRange(distance, _zombie.Stats.DetectDistance))
        {
            Agent.ResetPath();
            Agent.speed = _zombie.Stats.MoveSpeed;
            Animator.SetTrigger(ZombieAnimatorHash.TraceToComeback);
            TransitionTo(EZombieState.Comeback);
            return;
        }

        if (Agent.isOnOffMeshLink)
        {
            OffMeshLinkData linkData = Agent.currentOffMeshLinkData;
            JumpData jumpData = new JumpData(linkData.startPos, linkData.endPos);
            if (jumpData.EndPosition.y > jumpData.StartPosition.y)
            {
                Animator.SetTrigger(ZombieAnimatorHash.TraceToJump);
                Agent.isStopped = true;
                Agent.ResetPath();
                _zombie.TransitionTo(EZombieState.Jump, in jumpData);
            }
        }
    }
}
