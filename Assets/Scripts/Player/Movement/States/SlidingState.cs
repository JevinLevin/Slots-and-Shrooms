using System;
using PrimeTween;
using UnityEngine;

[Serializable]
public class SlidingSettings : StateSettings
{
    public float slideDuration = 2;
    public Vector2 slideSpeedRange = new(0.25f, 2f);
    public AnimationCurve slideCurve;
    public float reslideDelay = 0.25f;

}
public class SlidingState : MovementState
{
    public SlidingSettings Settings => stateMachine.SlidingSettings;

    public SlidingState(StateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = (PlayerMovement)stateMachine;
    }

    private Vector3 slideDirection;
    private float slideTimer;
    private float slideProgress;
    private Tween reslideTween;

    public override bool CanEnter()
    {
        return !reslideTween.isAlive;
    }

    public override void CheckTransitions()
    {
        if(!stateMachine.IsHoldingCrouch)
        {
            SwitchState(stateMachine.WalkingState);
            return;
        }

        if (slideProgress >= 1)
        {
            SwitchState(stateMachine.CrouchingState);
            return;
        }

        if(stateMachine.IsHoldingJump)
        {
            stateMachine.JumpingState.LastSpeedMultiplier = stateMachine.SprintingSettings.speedMultiplier;
            SwitchState(stateMachine.JumpingState);
            return;
        }
    }

    public override void OnEnter()
    {
        stateMachine.GetCamera.SetHeightOffsetOverTime(stateMachine.CrouchingSettings.heightOffset);
        stateMachine.SetHeight(stateMachine.BasePlayerHeight + stateMachine.CrouchingSettings.heightOffset);

        slideDirection = stateMachine.GetCamera.transform.forward;
        slideDirection.y = 0;
        slideDirection.Normalize();
        slideTimer = 0;
        slideProgress = 0;
    }

    public override void OnExit()
    {
        stateMachine.GetCamera.SetHeightOffsetOverTime(0);
        stateMachine.ResetHeight();
        reslideTween = Tween.Delay(Settings.reslideDelay);
    }

    public override void OnInit()
    {
    }

    public override void OnTick()
    {
        float currentT = Settings.slideCurve.Evaluate(slideProgress);
        float currentSpeed = Mathf.Lerp(Settings.slideSpeedRange.x, Settings.slideSpeedRange.y, 1-currentT) * Time.deltaTime;
        Vector3 currentVelocity = slideDirection * currentSpeed;
        stateMachine.SetVelocity(currentVelocity);

        slideTimer += Time.deltaTime;
        slideProgress = slideTimer / Settings.slideDuration;

        Debug.Log(slideProgress);
        Debug.Log(currentT);
        Debug.Log(currentSpeed);
    }

}
