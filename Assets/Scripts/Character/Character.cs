using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour
{
    private static readonly int FlipXAnim = Animator.StringToHash("FlipX");
    private static readonly int MoveXAnim = Animator.StringToHash("MoveX");
    private static readonly int GroundedAnim = Animator.StringToHash("Grounded");
    private static readonly int JumpAnim = Animator.StringToHash("Jump");

    private Animator _anim;
    private Rigidbody2D _rb;
    private InputReader _input;
    private BoxCollider2D _box;
    private Transform _tr;

    [Header("In Air")] [SerializeField] private float inAirSpeed = 0.0625f * 30;
    [SerializeField] private float maxFallSpeed = 9.81f * 4f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private float minJumpHeight = 3f;
    [SerializeField] private Timer coyoteTimer = new(0.1f);


    [Header("Run")] [SerializeField] private float runSpeed = 0.0625f * 50;
    [SerializeField] private float stopDelay = 0.1f;

    [Header("Collisions")] [SerializeField]
    private LayerMask whatIsGround;

    
    private Counter _idleFrameCounter;
    private bool _flipX;
    private Trigger _flipXAnimTrigger;
    
    private Trigger _jumpTrigger;
    private Trigger _jumpAnimTrigger;
    private bool _jumping;
    private float _endJumpMultiplier;
    private float _jumpSpeed;
    
    private RayCollision _groundCollision;
    private RayCollision _ceilCollision;
    private Transition _grounded;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _input = GetComponent<InputReader>();
        _box = GetComponent<BoxCollider2D>();
        _tr = transform;
    }

    private void Start()
    {
        InitializeOnGroundAssociated();
        InitializeInAirAssociated();
        InitializeCollisions();
    }

    private void InitializeOnGroundAssociated()
    {
        _idleFrameCounter = new Counter(Mathf.RoundToInt(stopDelay / Time.fixedDeltaTime));
    }

    private void InitializeInAirAssociated()
    {
        _endJumpMultiplier = Mathf.Sqrt(minJumpHeight / jumpHeight);
        var gravity = -Physics2D.gravity.y;
        _jumpSpeed = Mathf.Sqrt(2f * jumpHeight * gravity) + gravity * Time.fixedDeltaTime;
    }

    private void InitializeCollisions()
    {
        var bounds = _box.bounds;
        var extents = new Vector2(bounds.extents.x, bounds.size.y);
        _groundCollision = new[]
        {
            new Vector2(-extents.x, 0f),
            new Vector2(extents.x, 0f)
        }.ToRayCollision(Vector2.down, whatIsGround);
        _ceilCollision = new[]
        {
            new Vector2(-extents.x, extents.y),
            new Vector2(extents.x, extents.y)
        }.ToRayCollision(Vector2.up, whatIsGround);
    }

    private void Update()
    {
        Animate();
    }

    private void Animate()
    {
        _anim.SetBool(MoveXAnim, !_idleFrameCounter.Check());
        _anim.SetBool(GroundedAnim, _grounded.State);
        if (_grounded.State && _flipXAnimTrigger.Check())
        {
            _anim.SetTrigger(FlipXAnim);
        }

        if (_jumpAnimTrigger.Check())
        {
            _anim.SetTrigger(JumpAnim);
        }
    }

    private void FixedUpdate()
    {
        ProcessCommonPhysics();
        if (_grounded.State)
        {
            ProcessOnGroundPhysics();
        }
        else
        {
            ProcessInAirPhysics();
        }
    }

    private void ProcessCommonPhysics()
    {
        var flipX = _input.FlipX;
        if (_flipX != flipX)
        {
            _flipX = flipX;
            _tr.FlipX(_flipX);
            _flipXAnimTrigger.Reset();
        }
    }

    private void ProcessOnGroundPhysics()
    {
        var moveX = _input.MoveX;
        _rb.linearVelocityX = moveX * runSpeed;
        _idleFrameCounter.Count(moveX == 0f);
        
        _jumpTrigger.Reset();
        if (_input.Jump && _jumpTrigger.Check())
        {
            Jump();
            _grounded.State = false;
        }
        else
        {
            _grounded.State = _groundCollision.Colliding(_tr.position);
        }
    }

    private void ProcessInAirPhysics()
    {
        var moveX = _input.MoveX;
        _rb.linearVelocityX = moveX * inAirSpeed;

        if (_grounded.Changed())
        {
            coyoteTimer.Start();
        }

        if (_input.Jump && coyoteTimer.Check() && _jumpTrigger.Check())
        {
            Jump();
            return;
        }

        var velocityY = _rb.linearVelocityY;
        var position = _tr.position;
        if (velocityY <= 0.1f || !_jumping)
        {
            _rb.linearVelocityY = Mathf.Max(-maxFallSpeed, velocityY);
            _grounded.State = _groundCollision.Colliding(position);
            _jumping = false;
        }
        else if (!_input.JumpHold && _jumping)
        {
            _rb.linearVelocityY *= _endJumpMultiplier;
            _jumping = false;
        }
        else if (_ceilCollision.Colliding(position))
        {
            _jumping = false;
        }
    }

    private void Jump()
    {
        _rb.linearVelocityY = _jumpSpeed;
        _jumping = true;
        _jumpAnimTrigger.Reset();
    }
}