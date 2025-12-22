public class ZombieDeathState : ZombieStateBase, IAnimationEventHandler
{
    public ZombieDeathState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        _zombie.Agent.isStopped = true;
        _zombie.Agent.ResetPath();
    }

    public void OnAnimationEnd()
    {
        UnityEngine.Object.Destroy(_zombie.gameObject);
    }
}
