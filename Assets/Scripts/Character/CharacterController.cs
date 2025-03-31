using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Transform _transform;

    private StateManager<CharacterStates> _stateManager = new(
        new SortedDictionary<CharacterStates, IState>
        {
            [CharacterStates.Idle] = new CharacterIdleState(),
            [CharacterStates.Walking] = new CharacterWalkingState(),
        });

    private void Update()
    {
    }
}

public enum CharacterStates
{
    Idle = 0,
    Walking = 1,
    Jumping = 2,
}