using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    public State CurrentState => currentState;
    protected State currentState;

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        currentState?.CheckTransitions();
        currentState?.OnTick();
    }

    public bool SwitchState(State newState, State oldState)
    {

        if (newState == oldState)
            return false;

        // Debug.Log("Switch to " + newState.GetType().Name);

        if (!newState.CanEnter())
            return false;


        currentState?.OnExit();

        currentState = newState;
        currentState?.OnEnter();

        OnStateSwitched();

        return true;
    }

    protected abstract void OnStateSwitched();
}
