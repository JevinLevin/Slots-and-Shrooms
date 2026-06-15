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
    
}
