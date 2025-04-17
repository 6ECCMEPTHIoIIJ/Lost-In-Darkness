using UnityEngine;

public class CharacterJumpingState : CharacterInAirState
{
    private static readonly int JumpingAnim = Animator.StringToHash("Jumping");


    private AnimationCurve _jumpCurve;
    private float _jumpHeight;
    private float _jumpDuration;

    private Rect[] _ceilDetects;
    private LayerMask _whatIsCeil;

    private Transform _transform;
    private Rigidbody2D _rb;
    private Animator _anim;

    private float _enterTime;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_jumpCurve, _jumpHeight, _jumpDuration,
            _transform, _rb, _anim,
            _ceilDetects, _whatIsCeil) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(JumpingAnim, true);
        _enterTime = Time.fixedTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(JumpingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        _rb.linearVelocityY =
            (_jumpCurve.Evaluate((Time.fixedTime - _enterTime) / _jumpDuration) -
             _jumpCurve.Evaluate((Time.fixedTime - Time.fixedDeltaTime - _enterTime) / _jumpDuration))
            / Time.fixedDeltaTime * _jumpDuration * _jumpHeight;

        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Time.fixedTime - _enterTime > _jumpDuration
            || _rb.linearVelocityY > 0 && IsTouchingCeil())
        {
            SwitchState(CharacterStates.Falling);
        }
    }

    private bool IsTouchingCeil()
    {
        var transform2D = new Vector2(_transform.position.x, _transform.position.y);
        foreach (var detect in _ceilDetects)
        {
            var scaledPosition = new Vector2(detect.x * Mathf.Sign(_transform.localScale.x),
                detect.y * Mathf.Sign(_transform.localScale.y));
            var origin = transform2D + scaledPosition;
            var direction = detect.size.normalized;
            var distance = detect.size.magnitude;
            var hit = Physics2D.Raycast(origin, direction, distance, _whatIsCeil);
            if (!hit) continue;
            return true;
        }

        return false;
    }

    public new class Data : CharacterInAirState.Data
    {
        public AnimationCurve JumpCurve;
        public float JumpHeight;
        public float JumpDuration;
        public Animator Anim;
        public Rect[] CeilDetects;
        public LayerMask WhatIsCeil;

        public void Deconstruct(out AnimationCurve jumpCurve, out float jumpHeight, out float jumpDuration,
            out Transform transform, out Rigidbody2D rb, out Animator anim, out Rect[] ceilDetects,
            out LayerMask whatIsCeil)
        {
            jumpCurve = JumpCurve;
            jumpHeight = JumpHeight;
            jumpDuration = JumpDuration;
            transform = Transform;
            rb = Rb;
            anim = Anim;
            ceilDetects = CeilDetects;
            whatIsCeil = WhatIsCeil;
        }
    }
}