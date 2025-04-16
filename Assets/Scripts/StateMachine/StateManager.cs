using System;
using System.Collections.Generic;
using System.Linq;

public interface IState<T>
{
    public StateManager<T> StateManager { get; set; }
    public void OnInitialize();
    public void OnEnter();
    public void OnExit();
    public void OnUpdate();
    public void OnFixedUpdate();
    public void OnSubStateEnter(IState<T> subState);
    public void AddToSuperState(IState<T> superState);
}

public abstract class State<T> : IState<T>
{
    private IState<T> _currentSubState;
    private StateManager<T> _stateManager;

    protected bool IsActive;
    protected IState<T> SuperState;

    public StateManager<T> StateManager
    {
        get => _stateManager;
        set
        {
            _stateManager = value;
            if (SuperState != null)
            {
                SuperState.StateManager = value;
            }
        }
    }


    public virtual void OnInitialize()
    {
        SuperState?.OnInitialize();
    }

    public virtual void OnEnter()
    {
        SuperState?.OnSubStateEnter(this);
        IsActive = true;
    }

    public virtual void OnExit()
    {
        IsActive = false;
        _currentSubState?.OnExit();
    }

    public virtual void OnUpdate()
    {
        SuperState?.OnUpdate();
    }

    public virtual void OnFixedUpdate()
    {
        SuperState?.OnFixedUpdate();
    }

    public void OnSubStateEnter(IState<T> subState)
    {
        _currentSubState = subState;
        if (IsActive) return;
        OnEnter();
    }

    public void AddToSuperState(IState<T> superState)
    {
        SuperState = superState;
    }
}

public abstract class StateManager<T>
{
    protected readonly IState<T>[] States;
    private IState<T> _currentState;

    protected StateManager(StateTree<T> states)
    {
        States = states.ToArray();
        foreach (var state in States)
        {
            state.StateManager = this;
        }
    }

    public void OnInitialize()
    {
        foreach (var state in States)
        {
            state.OnInitialize();
        }
    }

    public void OnUpdate()
    {
        _currentState.OnUpdate();
    }

    public void OnFixedUpdate()
    {
        _currentState.OnFixedUpdate();
    }

    public void OnSwitchState(T stateId)
    {
        _currentState?.OnExit();
        _currentState = States[Convert.ToInt32(stateId)];
        _currentState.OnEnter();
    }
}

public class StateTree<T>
{
    private bool _isEndState;
    private readonly List<StateTree<T>> _subStates = new();
    private IState<T> _state;
    private T _stateId;

    public IState<T>[] ToArray()
    {
        if (_isEndState)
        {
            return new[] { _state };
        }

        var sortedDictionary = new SortedDictionary<T, IState<T>>();
        foreach (var subState in _subStates)
        {
            subState.ToSortedDictionary(sortedDictionary);
        }

        return sortedDictionary.Values.ToArray();
    }

    private void ToSortedDictionary(SortedDictionary<T, IState<T>> sortedDictionary)
    {
        if (_isEndState)
        {
            sortedDictionary[_stateId] = _state;
            return;
        }

        foreach (var subState in _subStates)
        {
            subState.ToSortedDictionary(sortedDictionary);
        }
    }

    public void ConnectStates()
    {
        if (_isEndState) return;
        foreach (var subState in _subStates)
        {
            subState.ConnectStates();
            subState._state.AddToSuperState(_state);
        }
    }

    public class Builder
    {
        private StateNode _stateNode = new()
        {
            Tree = new StateTree<T>()
        };

        public Builder AddSuperState(IState<T> state)
        {
            _stateNode.Tree._subStates.Add(new StateTree<T> { _state = state });
            _stateNode = new StateNode
            {
                Parent = _stateNode,
                Tree = _stateNode.Tree._subStates.Last()
            };
            return this;
        }

        public Builder AddSubState(T stateId, IState<T> state)
        {
            _stateNode.Tree._subStates.Add(new StateTree<T> { _state = state, _stateId = stateId, _isEndState = true });
            return this;
        }

        public Builder End()
        {
            _stateNode = _stateNode.Parent;
            return this;
        }

        public StateTree<T> Build()
        {
            while (_stateNode.Parent != null) _stateNode = _stateNode.Parent;
            _stateNode.Tree.ConnectStates();
            return _stateNode.Tree;
        }

        private class StateNode
        {
            public StateNode Parent { get; set; }
            public StateTree<T> Tree { get; set; }
        }
    }
}