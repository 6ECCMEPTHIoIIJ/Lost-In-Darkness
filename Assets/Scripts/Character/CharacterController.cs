using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterPhysicsManager))]
[RequireComponent(typeof(CharacterInputManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]

public class CharacterController : MonoBehaviour
{
    private Transform _transform;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private CharacterPhysicsManager _physics;
    private CharacterInputManager _input;

    private CharacterIdleState _idleState;
    private CharacterWalkingState _walkingState;

    private StateManager<CharacterStates> _stateManager;

    private void Awake()
    {
        _idleState = new CharacterIdleState();
        _walkingState = new CharacterWalkingState();
        _stateManager = new CharacterStateManager(
            new SortedDictionary<CharacterStates, State<CharacterStates>>
            {
                [CharacterStates.Idle] = _idleState,
                [CharacterStates.Walking] = _walkingState,
            });
    }

    private void Start()
    {
        _transform = transform;
        _physics = GetComponent<CharacterPhysicsManager>();
        _input = GetComponent<CharacterInputManager>();
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();

        _idleState.Input = _input;
        _idleState.Physics = _physics;
        _idleState.Animator = _animator;
        _idleState.Sprite = _sprite;

        _walkingState.Input = _input;
        _walkingState.Physics = _physics;
        _walkingState.Animator = _animator;
        _walkingState.Sprite = _sprite;

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