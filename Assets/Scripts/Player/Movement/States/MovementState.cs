using System;
using UnityEngine;

[Serializable]
public class MovementStateSettings
{

}

[Serializable]
public abstract class MovementState : State
{
    protected MovementState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}
