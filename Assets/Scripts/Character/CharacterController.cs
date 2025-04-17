using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
public class CharacterController : MonoBehaviour
{
    private readonly StateMachine<CharacterStates> _sm = new();

    private Rigidbody2D _rb;
    private Animator _anim;
    private InputManager _input;

    [SerializeField] private float beginWalkingDuration = 0.3f;
    [SerializeField] private float walkingMovementSpeed = 5f;
    [SerializeField] private float endWalkingDuration = 0.3f;

    [SerializeField] private AnimationCurve jumpCurve;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float jumpDuration = 1f;

    [SerializeField] private float inAirMovementSpeed = 3f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float maxFallSpeed = 18.62f;

    [SerializeField] private float inactionDuration = 5f;
    [SerializeField] private float scareDuration = 2f;

    [SerializeField] private float flipWalkingDuration = 0.6f;

    [SerializeField] private LayerMask whatIsGround;

    [SerializeField] private Rect[] floorDetects;

    [SerializeField] private Rect[] ceilDetects;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _input = GetComponent<InputManager>();

        _sm.Initialize(new SortedDictionary<CharacterStates, IState<CharacterStates>>
        {
            [CharacterStates.Idle] = new CharacterIdleState(),
            [CharacterStates.Walking] = new CharacterWalkingState(),
            [CharacterStates.BeginWalking] = new CharacterBeginWalkingState(),
            [CharacterStates.EndWalking] = new CharacterEndWalkingState(),
            [CharacterStates.FlipWalking] = new CharacterFlipWalkingState(),
            [CharacterStates.Scared] = new CharacterScaredState(),
            [CharacterStates.Falling] = new CharacterFallingState(),
            [CharacterStates.Jumping] = new CharacterJumpingState(),
        });
    }

    private void Start()
    {
        _sm.SetStateData(CharacterStates.Idle, new CharacterIdleState.Data
        {
            InactionDuration = inactionDuration,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.Scared, new CharacterScaredState.Data
        {
            ScareDuration = scareDuration,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Anim = _anim,
            Transform = transform,
        });
        _sm.SetStateData(CharacterStates.Walking, new CharacterWalkingState.Data
        {
            MovementSpeed = walkingMovementSpeed,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.BeginWalking, new CharacterBeginWalkingState.Data
        {
            MovementSpeed = walkingMovementSpeed,
            BeginWalkingDuration = beginWalkingDuration,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.EndWalking, new CharacterEndWalkingState.Data
        {
            EndWalkingDuration = endWalkingDuration,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.FlipWalking, new CharacterFlipWalkingState.Data
        {
            FlipDuration = flipWalkingDuration,
            MovementSpeed = walkingMovementSpeed,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.Falling, new CharacterFallingState.Data
        {
            MovementSpeed = inAirMovementSpeed,
            Gravity = gravity,
            MaxFallSpeed = maxFallSpeed,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb,
        });
        _sm.SetStateData(CharacterStates.Jumping, new CharacterJumpingState.Data
        {
            MovementSpeed = inAirMovementSpeed,
            WhatIsFloor = whatIsGround,
            FloorDetects = floorDetects,
            WhatIsCeil = whatIsGround,
            CeilDetects = ceilDetects,
            Input = _input,
            Transform = transform,
            Rb = _rb,
            JumpCurve = jumpCurve,
            JumpDuration = jumpDuration,
            JumpHeight = jumpHeight,
            Anim = _anim,
        });
        _sm.OnSwitchState(CharacterStates.Idle);
    }

    private void Update()
    {
        _sm.Update();
    }

    private void FixedUpdate()
    {
        _sm.FixedUpdate();
    }

    private void OnDrawGizmos()
    {
        foreach (var detectRay in ceilDetects)
        {
            Gizmos.DrawRay(transform.position + new Vector3(detectRay.x * transform.localScale.x, detectRay.y),
                new Vector3(detectRay.size.x, detectRay.size.y));
        }

        foreach (var detectRay in floorDetects)
        {
            Gizmos.DrawRay(transform.position + new Vector3(detectRay.x * transform.localScale.x, detectRay.y),
                new Vector3(detectRay.size.x, detectRay.size.y));
        }

        Gizmos.DrawLine(transform.position + new Vector3(-0.25f, jumpHeight),
            transform.position + new Vector3(0.3215f, jumpHeight));
    }
}

public enum CharacterStates
{
    Idle = 0,
    Scared,
    BeginWalking,
    Walking,
    EndWalking,
    FlipWalking,
    Jumping,
    Falling,
    Count,
}