public struct Trigger
{
    private bool _state;

    public void Reset()
    {
        _state = true;
    }

    public bool Check()
    {
        var state = _state;
        _state = false;
        return state;
    }
}