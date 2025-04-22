using UnityEngine;

public class CharacterJumpingState : CharacterInAirState
{
    private static readonly int JumpingAnim = Animator.StringToHash("Jumping");

    private Rect[] _ceilDetects;
    private LayerMask _whatIsCeil;

    private Transform _tr;
    private Rigidbody2D _rb;
    private Animator _anim;
    private IJumpBrain _jumpBr;

    private float _jumpSpeed;

    protected override CharacterStates Id => CharacterStates.Jumping;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _jumpBr = data.input;
        _tr = data.tr;
        _rb = data.rb;
        _anim = data.anim;
        _ceilDetects = data.ceilDetects;
        _whatIsCeil = data.whatIsGround;
        _jumpSpeed = Mathf.Sqrt(2f * data.jumpHeight * -Physics2D.gravity.y);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(JumpingAnim, true);
        _rb.linearVelocityY = _jumpSpeed;
        _jumpBr.OnJumpPerformed();
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(JumpingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        _rb.linearVelocityY += Physics2D.gravity.y * Time.fixedDeltaTime;
        if (!_jumpBr.JumpHold)
        {
            _rb.linearVelocityY /= Mathf.Sqrt(2f);
            SwitchState(CharacterStates.Falling);
        }
        else if (_rb.linearVelocityY < 0f || _rb.linearVelocityY > 0f && IsTouchingCeil())
        {
            SwitchState(CharacterStates.Falling);
        }
    }

    private bool IsTouchingCeil()
    {
        var transform2D = new Vector2(_tr.position.x, _tr.position.y);
        foreach (var detect in _ceilDetects)
        {
            var scaledPosition = new Vector2(detect.x * Mathf.Sign(_tr.localScale.x),
                detect.y * Mathf.Sign(_tr.localScale.y));
            var origin = transform2D + scaledPosition;
            var direction = detect.size.normalized;
            var distance = detect.size.magnitude;
            var hit = Physics2D.Raycast(origin, direction, distance, _whatIsCeil);
            if (!hit) continue;
            return true;
        }

        return false;
    }
}