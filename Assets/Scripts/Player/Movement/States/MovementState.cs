using System;
using UnityEngine;

[Serializable]
public class StateSettings
{

}

[Serializable]
public abstract class MovementState : State
{
    [HideInInspector] protected new PlayerMovement stateMachine;
    protected MovementState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}
