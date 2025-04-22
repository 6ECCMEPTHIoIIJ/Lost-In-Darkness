using System;
using System.Collections.Generic;
using System.Linq;

public interface IState<T, TData> where TData : struct where T : Enum
{
    public void OnInitialize(StateMachine<T, TData> sm);
    public void OnSetData(in TData data);
    public void OnEnter();
    public void OnExit();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void SwitchState(T to);
}

public interface IStateMachine<T, TData> where TData : struct where T : Enum
{
    public void Initialize(IDictionary<T, IState<T, TData>> states);
    public void SetData(TData data);
    public void Update();
    public void FixedUpdate();
    public void OnSwitchState(T to);
}

public class StateMachine<T, TData> : IStateMachine<T, TData> where TData : struct where T : Enum
{
    private IState<T, TData>[] _states;
    private IState<T, TData> _currentState;

    public void Initialize(IDictionary<T, IState<T, TData>> states)
    {
        _states = states.Values.ToArray();
        foreach (var state in _states)
        {
            state.OnInitialize(this);
        }
    }

    public void SetData(TData data)
    {
        foreach (var state in _states)
        {
            state.OnSetData(data);
        }
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

public abstract class State<T, TData> : IState<T, TData> where TData : struct where T : Enum
{
    private StateMachine<T, TData> _sm;

    protected bool Active;
    protected abstract T Id { get; }

    public void OnInitialize(StateMachine<T, TData> sm)
    {
        _sm = sm;
    }

    public virtual void OnSetData(in TData data)
    {
    }

    public virtual void OnEnter()
    {
        Active = true;
    }

    public virtual void OnExit()
    {
        Active = false;
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