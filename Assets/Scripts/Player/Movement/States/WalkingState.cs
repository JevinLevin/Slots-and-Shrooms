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

    public override void CheckTransitions()
    {
        base.CheckTransitions();
        // Check for idle
        if (!stateMachine.IsMoving)
        {
            if (SwitchState(stateMachine.IdleState))
                return;
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        stateMachine.PlayerAnimator.ToggleWalking(true);
        AudioSource movementAudio = stateMachine.GetComponent<AudioSource>();
        movementAudio.volume = 0.1f; 
        movementAudio.pitch = 1f;
        movementAudio.clip = stateMachine.WalkingAudio; 
        movementAudio.Play();
    }

    public override void OnExit()
    {
        base.OnExit();
        stateMachine.PlayerAnimator.ToggleWalking(false);
    }
}
