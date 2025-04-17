using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

public interface IState<T>
{
    public void OnInitialize(StateMachine<T> sm);
    public void OnSetData(object data);
    public void OnEnter();
    public void OnExit();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void SwitchState(T to);
}

public interface IStateMachine<T>
{
    public void Initialize(IDictionary<T, IState<T>> states);
    public void SetStateData(T state, object data);
    public void Update();
    public void FixedUpdate();
    public void OnSwitchState(T to);
}

public class StateMachine<T> : IStateMachine<T>
{
    private IState<T>[] _states;
    private IState<T> _currentState;

    public void Initialize(IDictionary<T, IState<T>> states)
    {
        _states = states.Values.ToArray();
        foreach (var state in _states)
        {
            state.OnInitialize(this);
        }
    }

    public void SetStateData(T state, object data)
    {
        _states[Convert.ToInt32(state)].OnSetData(data);
    }

    public void Update()
    {
        _currentState.OnUpdate();
    }

    public void FixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    public void OnSwitchState(T to)
    {
        _currentState?.OnExit();
        _currentState = _states[Convert.ToInt32(to)];
        _currentState.OnEnter();
    }
}

public abstract class State<T> : IState<T>
{
    protected bool IsActive;
    private StateMachine<T> _sm;

    public void OnInitialize(StateMachine<T> sm)
    {
        _sm = sm;
    }

    public virtual void OnSetData(object data)
    {
    }

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

    public void SwitchState(T to)
    {
        _sm.OnSwitchState(to);
    }
}