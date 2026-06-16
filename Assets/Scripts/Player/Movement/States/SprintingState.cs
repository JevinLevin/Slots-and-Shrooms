using System;
using UnityEngine;

[Serializable]
public class SprintingSettings : StateSettings
{
    public float speedMultiplier = 1;
    public float fovOffset = 15;

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

    public override bool CanEnter()
    {
        return !stateMachine.PlayerShooter.IsAiming;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.GetCamera.AdjustFOVOverTime(Settings.fovOffset);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void CheckTransitions()
    {
        base.CheckTransitions();
        if (!stateMachine.IsHoldingSprint || stateMachine.PlayerShooter.IsAiming)
        {
            SwitchState(stateMachine.WalkingState);
            return;
        }

        // Check for slide
        if (stateMachine.IsPressingCrouch)
        {
            if (SwitchState(stateMachine.SlidingState))
                return;
        }
    }
}
