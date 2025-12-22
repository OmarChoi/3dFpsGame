using UnityEngine;

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
        CoinFactory.Instance.SpawnCoins(_zombie.Stats.DropItems, _zombie.transform.position);
        Object.Destroy(_zombie.gameObject);
    }
}
