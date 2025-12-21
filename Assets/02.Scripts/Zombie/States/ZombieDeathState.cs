public class ZombieDeathState : ZombieStateBase, IAnimationEventHandler
{
    public ZombieDeathState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        // Terminal state - waits for OnDeathAnimationEnd() to destroy GameObject
    }

    protected override void OnUpdate()
    {
        // Do nothing - waiting for animation event
    }

    public void OnAnimationEnd()
    {
        UnityEngine.Object.Destroy(_zombie.gameObject);
    }
}
