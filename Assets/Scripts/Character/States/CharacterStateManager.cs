using System.Collections.Generic;

public class CharacterStateManager : StateManager<CharacterStates>
{
    public CharacterStateManager() :
        base(new SortedDictionary<CharacterStates, State<CharacterStates>>
        {
            [CharacterStates.Idle] = new CharacterIdleState(),
            [CharacterStates.Walking] = new CharacterWalkingState(),
            [CharacterStates.InAir] = new CharacterInAirState(),
            [CharacterStates.Flip] = new CharacterFlipState(),
            [CharacterStates.Scared] = new CharacterScaredState(),
            [CharacterStates.BeginWalking] = new CharacterBeginWalkingState(),
            [CharacterStates.EndWalking] = new CharacterEndWalkingState(),
        })
    {
    }
}

public abstract class CharacterState : State<CharacterStates>
{
    public CharacterController Controller { get; set; }
}

public enum CharacterStates
{
    Idle = 0,
    BeginWalking,
    Walking,
    EndWalking,
    Flip,
    InAir,
    Scared,
    Count,
}