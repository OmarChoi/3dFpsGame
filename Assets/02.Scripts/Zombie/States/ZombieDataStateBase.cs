using UnityEngine;

public abstract class ZombieDataStateBase<TData> : ZombieStateBase, IZombieDataState<TData> where TData : struct
{
    public override bool HasData => true;

    protected ZombieDataStateBase(Zombie zombie) : base(zombie) { }

    // Seal the parameterless OnEnter to prevent usage
    protected sealed override void OnEnter()
    {
        // This should never be called for data states
    }

    // Seal the parameterless Enter to prevent usage
    public sealed override void Enter()
    {
        
    }

    // Implement interface - public entry point with data
    public void EnterWithData(in TData data)
    {
        OnEnter(in data);
    }

    // TransitionTo overload for data states
    protected void TransitionTo<TNextData>(EZombieState state, in TNextData data) where TNextData : struct
    {
        _zombie.TransitionTo(state, in data);
    }

    // Abstract method requiring data
    protected abstract void OnEnter(in TData data);
}
