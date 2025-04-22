public struct Transition
{
    private bool _prevState;
    private bool _state;

    public bool State
    {
        get => _state;
        set
        {
            _prevState = _state;
            _state = value;
        }
    }

    public bool Changed()
    {
        return _prevState != _state;
    }
}