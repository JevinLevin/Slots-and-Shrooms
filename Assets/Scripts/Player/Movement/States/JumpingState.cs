using PrimeTween;
using System;
using UnityEngine;
[Serializable]
public class JumpingSettings : StateSettings
{
    public float speedMultiplier;
    public float jumpHeight;
    [Tooltip("Slight delay after jumping before player can land again")]
    public float groundingDelay = 0.1f;
}

public class JumpingState : InputMoveState
{
    public JumpingSettings Settings => stateMachine.JumpingSettings;

    public JumpingState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override float GetSpeedMultiplier()
    {
        float lastMultiplier = Mathf.Max(1, LastSpeedMultiplier);
        return Settings.speedMultiplier * lastMultiplier;
    }
    public float LastSpeedMultiplier;

    private Tween groundedTween;

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

        Debug.Log(jumpVelocity);
        Debug.Log(stateMachine.GetVelocity);

        groundedTween = Tween.Delay(Settings.groundingDelay);
    }

    public override void CheckTransitions()
    {
        if(!groundedTween.isAlive && stateMachine.IsGrounded)
        {
            SwitchState(stateMachine.WalkingState);
            return;
        }

        base.CheckTransitions();
    }


}
