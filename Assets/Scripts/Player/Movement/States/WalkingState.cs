using System;
using UnityEngine;

[Serializable]
public class WalkingSettings : StateSettings
{
    public float speedMultiplier = 1;

}
public class WalkingState : InputMoveState
{
    public WalkingSettings Settings => stateMachine.WalkingSettings;
    public override float GetSpeedMultiplier()
    {
        return Settings.speedMultiplier;
    }

    public WalkingState(StateMachine stateMachine) : base(stateMachine)
    {
    }
}
