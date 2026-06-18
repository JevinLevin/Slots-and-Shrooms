using System;
using UnityEngine;

[Serializable]
public class MovementSettings : StateSettings
{
    public float baseMoveSpeed = 10;
    public float aimingSpeedMultiplier = 0.5f;
}

public abstract class InputMoveState : MovementState
{
    public MovementSettings Settings => stateMachine.MovementSettings;
    public abstract float GetSpeedMultiplier();

    public InputMoveState(StateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = (PlayerMovement)stateMachine;
    }

    public override bool CanEnter()
    {
        return true;
    }

    public override void CheckTransitions()
    {
        // Check for jump
        if (stateMachine.IsGrounded && stateMachine.IsHoldingJump)
        {
            stateMachine.JumpingState.LastSpeedMultiplier = GetSpeedMultiplier();
            if(SwitchState(stateMachine.JumpingState))
                return;
        }

        // Check for sprint
        if (stateMachine.CurrentState is not JumpingState 
            && stateMachine.IsGrounded
            && stateMachine.IsHoldingSprint)
        {
            if(SwitchState(stateMachine.SprintingState))
                return;
        }

        // Check for crouch
        if(stateMachine.CurrentState is not SprintingState
            && stateMachine.IsGrounded 
            && stateMachine.IsHoldingCrouch)
        {
            if (SwitchState(stateMachine.CrouchingState))
                return;
        }
    }

    public override void OnEnter()
    {
        if(!stateMachine.PlayerShooter.IsAiming 
           && stateMachine.CurrentState is not SprintingState
           && stateMachine.CurrentState is not JumpingState)
            stateMachine.GetCamera.ResetFOVOverTime();
    }

    public override void OnExit()
    {
    }

    public override void OnInit()
    {
    }

    public override void OnTick()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputY = Input.GetAxisRaw("Vertical");
        Vector3 velocity = new(inputX, 0, inputY);
        velocity.Normalize();

        float multiplier = GetSpeedMultiplier();
        if (stateMachine.PlayerShooter.IsAiming)
            multiplier *= Settings.aimingSpeedMultiplier;

        multiplier *= stateMachine.BaseSpeedMultiplier;
        
        velocity *= Settings.baseMoveSpeed * multiplier;

        // Rotate velocity based on look direction
        velocity = stateMachine.GetCamera.transform.TransformDirection(velocity);


        velocity.y = stateMachine.GetVelocity.y;

        stateMachine.SetVelocity(velocity);
    }
}
