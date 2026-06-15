using System;
using UnityEngine;

[Serializable]
public class MovementSettings : StateSettings
{
    public float baseMoveSpeed = 10;
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
        if (stateMachine.IsGrounded && Input.GetKey(KeyCode.Space))
        {
            stateMachine.JumpingState.LastSpeedMultiplier = GetSpeedMultiplier();
            if(SwitchState(stateMachine.JumpingState))
                return;
        }

        // Check for sprint
        if (stateMachine.CurrentState is not JumpingState &&
            stateMachine.IsGrounded &&
            Input.GetKey(KeyCode.LeftShift))
        {
            if(SwitchState(stateMachine.SprintingState))
                return;
        }
    }

    public override void OnEnter()
    {
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


        velocity *= Settings.baseMoveSpeed * GetSpeedMultiplier();
        velocity *= Time.deltaTime;

        // Rotate velocity based on look direction
        velocity = stateMachine.GetCamera.transform.TransformDirection(velocity);


        velocity.y = stateMachine.GetVelocity.y;

        stateMachine.SetVelocity(velocity);
    }
}
