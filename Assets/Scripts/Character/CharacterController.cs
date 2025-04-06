using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterWalkingPhysicsManager))]
[RequireComponent(typeof(CharacterInAirPhysicsManager))]
[RequireComponent(typeof(CharacterInputManager))]
[RequireComponent(typeof(CharacterCollisionsDetector))]
[RequireComponent(typeof(CharacterAnimationManager))]

public class CharacterController : MonoBehaviour
{
    private Transform _transform;
    private CharacterAnimationManager _animation;
    private CharacterWalkingPhysicsManager _walkingPhysics;
    private CharacterInAirPhysicsManager _inAirPhysics;
    private CharacterInputManager _input;
    private CharacterCollisionsDetector _collisions;

    private CharacterIdleState _idleState;
    private CharacterWalkingState _walkingState;
    private CharacterInAirState _inAirState;

    private StateManager<CharacterStates> _stateManager;

    private void Awake()
    {
        _idleState = new CharacterIdleState();
        _walkingState = new CharacterWalkingState();
        _inAirState = new CharacterInAirState();
        _stateManager = new CharacterStateManager(
            new SortedDictionary<CharacterStates, State<CharacterStates>>
            {
                [CharacterStates.Idle] = _idleState,
                [CharacterStates.Walking] = _walkingState,
                [CharacterStates.InAir] = _inAirState,
            });
    }

    private void Start()
    {
        _transform = transform;
        _walkingPhysics = GetComponent<CharacterWalkingPhysicsManager>();
        _inAirPhysics = GetComponent<CharacterInAirPhysicsManager>();
        _input = GetComponent<CharacterInputManager>();
        _animation = GetComponent<CharacterAnimationManager>();
        _collisions = GetComponent<CharacterCollisionsDetector>();

        _idleState.Input = _input;
        _idleState.WalkingPhysics = _walkingPhysics;
        _idleState.InAirPhysics = _inAirPhysics;
        _idleState.Animation = _animation;
        _idleState.Collisions = _collisions;

        _walkingState.Input = _input;
        _walkingState.WalkingPhysics = _walkingPhysics;
        _walkingState.InAirPhysics = _inAirPhysics;
        _walkingState.Animation = _animation;
        _walkingState.Collisions = _collisions;
        
        _inAirState.InAirPhysics = _inAirPhysics;
        _inAirState.Collisions = _collisions;

        _stateManager.OnSwitchState(CharacterStates.Idle);
    }

    private void Update()
    {
        _stateManager.OnUpdate();
    }

    private void FixedUpdate()
    {
        _stateManager.OnFixedUpdate();
    }
}