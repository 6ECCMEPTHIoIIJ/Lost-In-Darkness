using UnityEngine;

public abstract class CharacterGroundedState : State<CharacterStates>
{
    private Rect[] _groundDetects;
    private LayerMask _whatIsGround;

    private Transform _transform;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_transform, _groundDetects, _whatIsGround) = (Data)data;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (!IsGrounded())
        {
            SwitchState(CharacterStates.InAir);
        }
    }

    private bool IsGrounded()
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
            return true;
        }

        return false;
    }

    public abstract class Data
    {
        public Transform Transform;
        public Rect[] GroundDetects;
        public LayerMask WhatIsGround;

        public void Deconstruct(out Transform transform, out Rect[] groundDetects, out LayerMask whatIsGround)
        {
            transform = Transform;
            groundDetects = GroundDetects;
            whatIsGround = WhatIsGround;
        }
    }
}