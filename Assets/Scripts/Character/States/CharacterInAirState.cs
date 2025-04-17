using UnityEngine;

public abstract class CharacterInAirState : State<CharacterStates>
{
    private float _movementSpeed;
    private Rect[] _floorDetects;
    private LayerMask _whatIsFloor;

    private Transform _transform;
    private InputManager _input;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_movementSpeed, _floorDetects, _whatIsFloor, _rb, _input, _transform) = (Data)data;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        _rb.linearVelocityX = _input.MovementDirection * _movementSpeed;
        if (_rb.linearVelocityY < 0.1f && IsTouchingFloor(out var groundPoint))
        {
            _transform.position = new Vector2(_transform.position.x, groundPoint.y);
            _rb.linearVelocityY = 0;
            SwitchState(_input.MovementDirection == 0
                ? CharacterStates.Idle
                : CharacterStates.Walking
            );
        }
    }

    private bool IsTouchingFloor(out Vector2 groundPoint)
    {
        var transform2D = new Vector2(_transform.position.x, _transform.position.y);
        foreach (var detect in _floorDetects)
        {
            var scaledPosition = new Vector2(detect.x * Mathf.Sign(_transform.localScale.x),
                detect.y * Mathf.Sign(_transform.localScale.y));
            var origin = transform2D + scaledPosition;
            var direction = detect.size.normalized;
            var distance = detect.size.magnitude;
            var hit = Physics2D.Raycast(origin, direction, distance, _whatIsFloor);
            if (!hit) continue;
            groundPoint = hit.point;
            return true;
        }

        groundPoint = default;
        return false;
    }

    public abstract class Data
    {
        public float MovementSpeed;

        public Rect[] FloorDetects;
        public LayerMask WhatIsFloor;

        public Rigidbody2D Rb;
        public InputManager Input;
        public Transform Transform;

        public void Deconstruct(out float movementSpeed, out Rect[] floorDetects, out LayerMask whatIsFloor,
            out Rigidbody2D rb, out InputManager input, out Transform transform)
        {
            movementSpeed = MovementSpeed;
            floorDetects = FloorDetects;
            whatIsFloor = WhatIsFloor;
            rb = Rb;
            input = Input;
            transform = Transform;
        }
    }
}