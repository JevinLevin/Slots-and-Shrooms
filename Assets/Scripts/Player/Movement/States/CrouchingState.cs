using System;
using UnityEngine;

[Serializable]
public class CrouchingSettings : StateSettings
{
    public float speedMultiplier = 1;
    public float heightOffset = -0.5f;

}

public class CrouchingState : InputMoveState
{
    public CrouchingSettings Settings => stateMachine.CrouchingSettings;
    public override float GetSpeedMultiplier()
    {
        return Settings.speedMultiplier * stateMachine.CrouchSpeedMultiplier;
    }

    public CrouchingState(StateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.GetCamera.SetHeightOffsetOverTime(Settings.heightOffset);
        stateMachine.SetHeight(stateMachine.BasePlayerHeight + Settings.heightOffset);
        AudioSource movementAudio = stateMachine.GetComponent<AudioSource>();
        movementAudio.pitch = 0.6f;
        movementAudio.volume = 0.1f;
        movementAudio.loop = true;
        movementAudio.clip = stateMachine.WalkingAudio;
        movementAudio.Play();
    }

    public override void OnExit()
    {
        base.OnExit();
        stateMachine.GetCamera.SetHeightOffsetOverTime(0);
        stateMachine.ResetHeight();
    }

    public override void CheckTransitions()
    {
        base.CheckTransitions();

        if (!Input.GetKey(KeyCode.LeftControl)
            && CanUncrouch())
        {
            if (SwitchState(stateMachine.WalkingState))
                return;
        }
    }

    private bool CanUncrouch()
    {
        // Check that there arent any collisions above
        float checkHeight = stateMachine.BasePlayerHeight - (stateMachine.BasePlayerWidth / 2);
        return !stateMachine.CheckSphere(checkHeight);

    }
}
