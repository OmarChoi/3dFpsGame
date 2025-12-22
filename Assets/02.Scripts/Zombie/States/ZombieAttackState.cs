public class ZombieAttackState : ZombieStateBase, IAnimationEventHandler
{
    private float _attackTimer;
    private bool _isAttacking;

    public ZombieAttackState(Zombie zombie) : base(zombie) { }

    protected override void OnEnter()
    {
        _zombie.StopAgent();
        _attackTimer = -_zombie.Stats.AttackInterval;
        _isAttacking = false;
    }

    protected override void OnUpdate()
    {
        if (!Transform.IsInRange(Player.position, _zombie.Stats.AttackDistance))
        {
            Animator.SetTrigger(ZombieAnimatorHash.AttackToTrace);
            TransitionTo(EZombieState.Trace);
            return;
        }

        if (_isAttacking) return;

        _attackTimer += UnityEngine.Time.deltaTime;
        if (_attackTimer >= _zombie.Stats.AttackInterval)
        {
            _isAttacking = true;
            Animator.SetTrigger(ZombieAnimatorHash.Attack);
        }
    }

    public void OnAnimationEnd()
    {
        _isAttacking = false;
        _attackTimer = 0;
    }
}
