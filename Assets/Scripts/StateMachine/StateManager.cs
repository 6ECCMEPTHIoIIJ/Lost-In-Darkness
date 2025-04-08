using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StateManager<T> where T : Enum
{
    private State<T> _currentState;

    private readonly State<T>[] _states;

    public StateManager(SortedDictionary<T, State<T>> statesTable)
    {
        _states = statesTable.Values.ToArray();
        foreach (var state in _states)
        {
            state.StateManager = this;
        }
    }

    public void OnInitialize()
    {
        foreach (var state in _states)
        {
            state.OnInitialize();
        }
    }

    public void OnUpdate()
    {
        _currentState?.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _currentState?.OnFixedUpdate();
    }


    public void OnSwitchState(T stateId)
    {
        _currentState?.OnExit();
        _currentState = _states[Convert.ToInt32(stateId)];
        _currentState.OnEnter();
    }

    public void ForEachState(Action<State<T>> action)
    {
        foreach (var state in _states)
        {
            action(state);
        }
    }
}

public abstract class State<T> where T : Enum
{
    protected bool IsActive { get; private set; }
    public StateManager<T> StateManager { get; set; }
    
    public virtual void OnInitialize() {}

    public virtual void OnEnter()
    {
        IsActive = true;
    }

    public virtual void OnExit()
    {
        IsActive = false;
    }

    public virtual void OnUpdate()
    {
    }

    public virtual void OnFixedUpdate()
    {
    }
}