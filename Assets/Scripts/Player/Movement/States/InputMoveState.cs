using System;
using UnityEngine;

[Serializable]
public class InputMoveStateSettings : MovementStateSettings
{
    public float speedMultiplier = 1;
}

public abstract class InputMoveState : MovementState
{
    [SerializeField] protected float speedMultiplier;

    public InputMoveStateSettings Settings;

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
        Debug.Log(inputX + "," + inputY);
    }
}
