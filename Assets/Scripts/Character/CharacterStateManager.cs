public class CharacterStateManager : StateManager<CharacterStates>
{
    public CharacterController Controller
    {
        set
        {
            foreach (var state in States)
            {
                (state as ICharacterState)!.Controller = value;
            }
        }
    }

    public CharacterStateManager() : base(
        new StateTree<CharacterStates>.Builder()
            .AddSuperState(new CharacterGroundedState())
            .AddSuperState(new CharacterGroundedMoveState())
            .AddSubState(CharacterStates.Walking, new CharacterWalkingState())
            .AddSubState(CharacterStates.BeginWalking, new CharacterBeginWalkingState())
            .AddSubState(CharacterStates.EndWalking, new CharacterEndWalkingState())
            .End() // CharacterGroundedMoveState
            .AddSubState(CharacterStates.Scared, new CharacterScaredState())
            .AddSubState(CharacterStates.Idle, new CharacterIdleState())
            .AddSubState(CharacterStates.FlipWalking, new CharacterFlipWalkingState())
            .End() // CharacterGroundedState
            .AddSubState(CharacterStates.InAir, new CharacterInAirState())
            .Build()
    )
    {
    }
}

public interface ICharacterState : IState<CharacterStates>
{
    public CharacterController Controller { get; set; }
}

public class CharacterState : State<CharacterStates>, ICharacterState
{
    private CharacterController _controller;

    public CharacterController Controller
    {
        get => _controller;
        set
        {
            _controller = value;
            if (SuperState != null)
            {
                (SuperState as ICharacterState)!.Controller = value;
            }
        }
    }
}

public enum CharacterStates
{
    Idle = 0,
    BeginWalking,
    Walking,
    EndWalking,
    FlipWalking,
    InAir,
    Scared,
    Count,
}