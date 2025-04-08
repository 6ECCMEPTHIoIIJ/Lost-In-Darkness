using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterPhysicsManager))]
[RequireComponent(typeof(CharacterInputManager))]
[RequireComponent(typeof(CharacterCollisionsDetector))]
[RequireComponent(typeof(CharacterAnimationManager))]
[RequireComponent(typeof(CharacterEffectsManager))]
public class CharacterController : MonoBehaviour
{
    private CharacterStateManager _stateManager;

    public Transform Transform { get; private set; }
    public CharacterAnimationManager Animation { get; private set; }
    public CharacterPhysicsManager Physics { get; private set; }
    public CharacterInputManager Input { get; private set; }
    public CharacterCollisionsDetector Collisions { get; private set; }
    public CharacterEffectsManager Effects { get; private set; }

    private void Awake()
    {
        Transform = transform;
        Physics = GetComponent<CharacterPhysicsManager>();
        Input = GetComponent<CharacterInputManager>();
        Animation = GetComponent<CharacterAnimationManager>();
        Collisions = GetComponent<CharacterCollisionsDetector>();
        Effects = GetComponent<CharacterEffectsManager>();

        _stateManager = new CharacterStateManager();
    }

    private void Start()
    {
        _stateManager.ForEachState(state => (state as CharacterState)!.Controller = this);
        _stateManager.OnInitialize();
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