public interface IMoveXBrain
{
    public float MoveX { get; }
    public bool FlipX { get; }
}

public interface IMoveYBrain
{
    public float MoveY { get; }
    public bool FlipY { get; }
}

public interface IJumpBrain
{
    public bool Jump { get; }
    public bool JumpHold { get; }
    public void OnJumpPerformed();
}