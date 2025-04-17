using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputManager))]
public class CharacterController : MonoBehaviour
{
    private readonly StateMachine<CharacterStates> _sm = new();

    private Rigidbody2D _rb;
    private Animator _anim;
    private InputManager _input;

    [SerializeField] private float beginWalkingDelay = 0.2f;
    [SerializeField] private float beginWalkingDuration = 0.3f;
    [SerializeField] private float walkingMovementSpeed = 5f;
    [SerializeField] private float endWalkingDuration = 0.3f;

    [SerializeField] private float inAirMovementSpeed = 3f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float maxFallSpeed = 18.62f;

    [SerializeField] private float inactionDuration = 5f;
    [SerializeField] private float scareDuration = 2f;

    [SerializeField] private float flipWalkingDuration = 0.6f;

    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Rect[] groundDetects;

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
            [CharacterStates.InAir] = new CharacterInAirState(),
        });
    }

    private void Start()
    {
        _sm.SetStateData(CharacterStates.Idle, new CharacterIdleState.Data
        {
            InactionDuration = inactionDuration,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
        });
        _sm.SetStateData(CharacterStates.Scared, new CharacterScaredState.Data
        {
            ScareDuration = scareDuration,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Anim = _anim,
            Transform = transform,
        });
        _sm.SetStateData(CharacterStates.Walking, new CharacterWalkingState.Data
        {
            MovementSpeed = walkingMovementSpeed,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.BeginWalking, new CharacterBeginWalkingState.Data
        {
            MovementSpeed = walkingMovementSpeed,
            BeginWalkingDelay = beginWalkingDelay,
            BeginWalkingDuration = beginWalkingDuration,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.EndWalking, new CharacterEndWalkingState.Data
        {
            EndWalkingDuration = endWalkingDuration,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.FlipWalking, new CharacterFlipWalkingState.Data
        {
            FlipDuration = flipWalkingDuration,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb
        });
        _sm.SetStateData(CharacterStates.InAir, new CharacterInAirState.Data
        {
            MovementSpeed = inAirMovementSpeed,
            Gravity = gravity,
            MaxFallSpeed = maxFallSpeed,
            WhatIsGround = whatIsGround,
            GroundDetects = groundDetects,
            Input = _input,
            Anim = _anim,
            Transform = transform,
            Rb = _rb,
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
        foreach (var detectRay in groundDetects)
        {
            Gizmos.DrawRay(transform.position + new Vector3(detectRay.x, detectRay.y),
                new Vector3(detectRay.size.x, detectRay.size.y));
        }
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
    InAir,
    Count,
}