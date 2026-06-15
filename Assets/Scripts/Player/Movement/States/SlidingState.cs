using System;
using UnityEngine;

[Serializable]
public class SlidingSettings : StateSettings
{
    public float slideDuration = 2;
    public Vector2 slideSpeedRange = new(5, 0.25f);
    public AnimationCurve slideCurve;

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

    public override bool CanEnter()
    {
        return true;
    }

    public override void CheckTransitions()
    {
        if(!Input.GetKey(KeyCode.LeftControl))
        {
            SwitchState(stateMachine.WalkingState);
            return;
        }

        if (slideProgress >= 1)
        {
            SwitchState(stateMachine.CrouchingState);
            return;
        }

        if(Input.GetKeyDown(KeyCode.Space))
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
    }

    public override void OnInit()
    {
    }

    public override void OnTick()
    {
        float currentT = Settings.slideCurve.Evaluate(slideProgress);
        float currentSpeed = Mathf.Lerp(Settings.slideSpeedRange.y, Settings.slideSpeedRange.x, currentT);
        Vector3 currentVelocity = slideDirection * currentSpeed;
        stateMachine.SetVelocity(currentVelocity);

        slideTimer += Time.deltaTime;
        slideProgress = slideTimer / Settings.slideDuration;
        Debug.Log(slideProgress);
        Debug.Log(currentT);
        Debug.Log(currentSpeed);
    }

}
