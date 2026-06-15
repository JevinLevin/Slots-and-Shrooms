using System;
using UnityEngine;

[Serializable]
public class SprintingSettings : StateSettings
{
    public float speedMultiplier = 1;

}
public class SprintingState : InputMoveState
{
    public SprintingSettings Settings => stateMachine.SprintingSettings;
    public override float GetSpeedMultiplier()
    {
        return Settings.speedMultiplier;
    }

    public SprintingState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void CheckTransitions()
    {
        base.CheckTransitions();
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            SwitchState(stateMachine.WalkingState);
            return;
        }
    }
}
