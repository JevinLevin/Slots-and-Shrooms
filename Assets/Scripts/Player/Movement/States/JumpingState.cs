using System;
using UnityEngine;
[Serializable]
public class JumpingSettings : StateSettings
{
    public float speedMultiplier;
    public float jumpHeight;
}

public class JumpingState : InputMoveState
{
    public JumpingSettings Settings => stateMachine.JumpingSettings;
    public override float GetSpeedMultiplier()
    {
        return Settings.speedMultiplier;
    }

    public JumpingState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override bool CanEnter()
    {
        return stateMachine.IsGrounded;
    }

    public override void OnEnter()
    {
        base.OnEnter();

        Vector3 jumpVelocity = Vector3.zero;
        jumpVelocity.y = Settings.jumpHeight;
        stateMachine.ImpulseVelocity(jumpVelocity);
    }

    public override void CheckTransitions()
    {
        if(stateMachine.IsGrounded)
        {
            SwitchState(stateMachine.WalkingState);
            return;
        }

        base.CheckTransitions();
    }


}
