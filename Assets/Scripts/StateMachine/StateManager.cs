using System;
using System.Collections.Generic;
using System.Linq;

public class StateManager<T> where T : Enum
{
    public IState CurrentState { get; private set; }

    public IState[] States { get; private set; }

    public StateManager(SortedDictionary<T, IState> statesTable)
    {
        States = statesTable.Values.ToArray();
    }

    public void OnUpdate(float deltaTime)
    {
        CurrentState?.OnUpdate(deltaTime);
    }

    public void OnFixedUpdate(float fixedDeltaTime)
    {
        CurrentState?.OnFixedUpdate(fixedDeltaTime);
    }


    public void OnSwitchState(T stateId)
    {
        CurrentState?.OnExit();
        CurrentState = States[Convert.ToInt32(stateId)];
        CurrentState?.OnEnter();
    }
}

public interface IState
{
    public void OnEnter();
    public void OnExit();
    public void OnUpdate(float deltaTime);
    public void OnFixedUpdate(float fixedDeltaTime);
}