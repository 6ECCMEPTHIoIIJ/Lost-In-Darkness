using UnityEngine;

public abstract class CharacterGroundedState : State<CharacterStates>
{
    private Rect[] _floorDetects;
    private LayerMask _whatIsFloor;

    private Transform _transform;
    private InputManager _input;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_transform, _floorDetects, _whatIsFloor, _input) = (Data)data;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (_input.IsJumping)
        {
            SwitchState(CharacterStates.Jumping);
        }
        else if (!IsTouchingFloor())
        {
            SwitchState(CharacterStates.Falling);
        }
    }

    private bool IsTouchingFloor()
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
            return true;
        }

        return false;
    }

    public abstract class Data
    {
        public Transform Transform;
        public Rect[] FloorDetects;
        public LayerMask WhatIsFloor;
        public InputManager Input;

        public void Deconstruct(out Transform transform, out Rect[] floorDetects, out LayerMask whatIsFloor,
            out InputManager input)
        {
            transform = Transform;
            floorDetects = FloorDetects;
            whatIsFloor = WhatIsFloor;
            input = Input;
        }
    }
}