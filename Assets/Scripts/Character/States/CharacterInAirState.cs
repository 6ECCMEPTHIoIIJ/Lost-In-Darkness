using System;
using UnityEngine;

public class CharacterInAirState : State<CharacterStates>
{
    private static readonly int FallingAnim = Animator.StringToHash("Falling");

    private float _movementSpeed;
    private float _gravity;
    private float _maxFallSpeed;

    private Rect[] _groundDetects;
    private LayerMask _whatIsGround;

    private Transform _transform;
    private Rigidbody2D _rb;
    private InputManager _input;
    private Animator _anim;

    private Vector2 _groundPoint;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_movementSpeed, _gravity, _maxFallSpeed,
            _groundDetects, _whatIsGround,
            _transform, _rb, _input, _anim) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(FallingAnim, true);
    }

    public override void OnExit(CharacterStates to)
    {
        base.OnExit(to);
        _anim.SetBool(FallingAnim, false);

        if (to == CharacterStates.Idle)
        {
            _transform.position = new Vector2(_transform.position.x, _groundPoint.y);
            _rb.linearVelocityY = 0;
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        _rb.linearVelocityX = _input.MovementDirection * _movementSpeed;
        _rb.linearVelocityY = Mathf.Max(-_maxFallSpeed, _rb.linearVelocityY - _gravity);
        if (IsGrounded(out _groundPoint))
        {
            SwitchState(CharacterStates.Idle);
        }
    }

    private bool IsGrounded(out Vector2 groundPoint)
    {
        var transform2D = new Vector2(_transform.position.x, _transform.position.y);
        foreach (var groundDetect in _groundDetects)
        {
            var scaledPosition = new Vector2(groundDetect.x * Mathf.Sign(_transform.localScale.x),
                groundDetect.y * Mathf.Sign(_transform.localScale.y));
            var origin = transform2D + scaledPosition;
            var direction = groundDetect.size.normalized;
            var distance = groundDetect.size.magnitude;
            var hit = Physics2D.Raycast(origin, direction, distance, _whatIsGround);
            if (!hit) continue;
            groundPoint = hit.point;
            return true;
        }

        groundPoint = default;
        return false;
    }

    public class Data
    {
        public float MovementSpeed;
        public float Gravity;
        public float MaxFallSpeed;

        public Rect[] GroundDetects;
        public LayerMask WhatIsGround;

        public Transform Transform;
        public Rigidbody2D Rb;
        public InputManager Input;
        public Animator Anim;

        public void Deconstruct(out float movementSpeed, out float gravity, out float maxFallSpeed,
            out Rect[] groundDetects, out LayerMask whatIsGround, out Transform transform, out Rigidbody2D rb,
            out InputManager input, out Animator anim)
        {
            movementSpeed = MovementSpeed;
            gravity = Gravity;
            maxFallSpeed = MaxFallSpeed;
            groundDetects = GroundDetects;
            whatIsGround = WhatIsGround;
            transform = Transform;
            rb = Rb;
            input = Input;
            anim = Anim;
        }
    }
}