using UnityEngine;

public class PlayerMovement : StateMachine
{

    [SerializeField] private WalkingState walkingState;
    [SerializeField] private WalkingStateSettings walkingStateSettings;

    protected override void Awake()
    {
        base.Awake();

        walkingState = new WalkingState(this);
        walkingState.Settings = walkingStateSettings;

        currentState = walkingState;
    }

    protected override void OnStateSwitched()
    {
    }
}
