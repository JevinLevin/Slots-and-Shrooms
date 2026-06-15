using System;
using UnityEngine;

[Serializable]
public class WalkingStateSettings : InputMoveStateSettings
{

}
public class WalkingState : InputMoveState
{
    public WalkingState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}
