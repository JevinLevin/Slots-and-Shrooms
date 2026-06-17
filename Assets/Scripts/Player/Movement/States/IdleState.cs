using System;
using UnityEngine;


[Serializable]
public class IdleSettings : StateSettings
{


}
public class IdleState : InputMoveState
{
    public IdleSettings Settings => stateMachine.IdleSettings;
    public override float GetSpeedMultiplier()
    {
        return 0;
    }
    public IdleState(StateMachine stateMachine) : base(stateMachine)
    {
        this.stateMachine = (PlayerMovement)stateMachine;
    }


    public override void CheckTransitions()
    {
        base.CheckTransitions();
        if (stateMachine.IsMoving)
            SwitchState(stateMachine.WalkingState);
    }

}
