using UnityEngine;
using UnityEngine.AI;

public abstract class ZombieStateBase
{
    protected readonly Zombie _zombie;
    protected Coroutine _activeCoroutine;
    
    public virtual bool HasData => false;
    protected NavMeshAgent Agent     => _zombie.Agent;
    protected Animator     Animator  => _zombie.Animator;
    protected Transform    Player    => _zombie.Player;
    protected Transform    Transform => _zombie.transform;
    protected ZombieMovement Movement => _zombie.Movement;
    
    protected abstract void OnEnter();
    protected abstract void OnUpdate();
    protected virtual void OnExit() { }
    
    protected ZombieStateBase(Zombie zombie)
    {
        _zombie = zombie;
    }

    public virtual void Enter()
    {
        OnEnter();
    }

    public void Update()
    {
        OnUpdate();
    }

    public void Exit()
    {
        OnExit();
        if (_activeCoroutine != null)
        {
            _zombie.StopCoroutine(_activeCoroutine);
            _activeCoroutine = null;
        }
    }

    protected void TransitionTo(EZombieState state)
    {
        _zombie.TransitionTo(state);
    }
}
