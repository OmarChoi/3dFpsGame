public class ZombieJumpState : ZombieStateBase
{
    public ZombieJumpState(Zombie zombie) : base(zombie) { }
    
    protected override void OnEnter()
    {
        JumpData jumpData = _zombie.PendingJumpData;
        _zombie.Movement.ExecuteJump(jumpData, OnJumpComplete);
    }
    
    private void OnJumpComplete()
    {
        Animator.SetTrigger(ZombieAnimatorHash.JumpToTrace);
        TransitionTo(EZombieState.Trace);
    }
}
