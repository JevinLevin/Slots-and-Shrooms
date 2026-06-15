using System;
using UnityEngine;

[Serializable]
public abstract class State
{
    [HideInInspector] protected StateMachine stateMachine;

    public State(StateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public abstract void OnInit();
    public abstract void OnEnter();
    public abstract void OnExit();
    public abstract void OnTick();
    public abstract void CheckTransitions();
    public abstract bool CanEnter();

    protected bool SwitchState(State newState)
    {
        if (newState == this)
            return false;

        //Debug.Log($"Switching to state: {newState.GetType().Name}");
        return stateMachine.SwitchState(newState, this);
    }
}
