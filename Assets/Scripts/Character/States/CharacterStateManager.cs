using System.Collections.Generic;

public class CharacterStateManager : StateManager<CharacterStates>
{
    public CharacterStateManager(SortedDictionary<CharacterStates, State<CharacterStates>> statesTable) :
        base(statesTable)
    {
    }
}

public abstract class CharacterState : State<CharacterStates>
{
}

public enum CharacterStates
{
    Idle = 0,
    Walking,
    InAir,
    Count,
}