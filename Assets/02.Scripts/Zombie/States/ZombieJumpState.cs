public class ZombieJumpState : ZombieDataStateBase<JumpData>
{
    public ZombieJumpState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter(in JumpData data)
    {
        Movement.ExecuteJump(in data, OnJumpComplete);
    }

    protected override void OnUpdate()
    {
        // Jump execution is handled by ZombieMovement.ExecuteJump
    }

    private void OnJumpComplete()
    {
        Animator.SetTrigger(ZombieAnimatorHash.JumpToTrace);
        TransitionTo(EZombieState.Trace);
    }
}
