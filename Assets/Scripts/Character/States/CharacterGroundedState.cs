using UnityEngine;

public abstract class CharacterGroundedState : State<CharacterStates, CharacterStateData>
{
    private float _runSpeed;

    private IMoveXBrain _moveBr;
    private IJumpBrain _jumpBr;
    private Transform _tr;
    private Rigidbody2D _rb;

    private Rect[] _floorDetects;
    private LayerMask _whatIsFloor;

    private bool _flipX;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _moveBr = data.input;
        _jumpBr = data.input;
        _tr = data.tr;
        _rb = data.rb;
        _runSpeed = data.runSpeed;
        _floorDetects = data.floorDetects;
        _whatIsFloor = data.whatIsGround;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _flipX = _moveBr.FlipX;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;
        if (Id == CharacterStates.Scared) return;

        _rb.linearVelocityX = _moveBr.MoveX * _runSpeed;
        if (_jumpBr.Jump)
        {
            SwitchState(CharacterStates.Jumping);
        }
        else if (!IsTouchingFloor())
        {
            SwitchState(CharacterStates.Falling);
        }
        else if (_flipX != _moveBr.FlipX)
        {
            SwitchState(CharacterStates.Flip);
        }
    }

    private bool IsTouchingFloor()
    {
        var transform2D = new Vector2(_tr.position.x, _tr.position.y);
        foreach (var detect in _floorDetects)
        {
            var scaledPosition = new Vector2(detect.x * Mathf.Sign(_tr.localScale.x),
                detect.y * Mathf.Sign(_tr.localScale.y));
            var origin = transform2D + scaledPosition;
            var direction = detect.size.normalized;
            var distance = detect.size.magnitude;
            var hit = Physics2D.Raycast(origin, direction, distance, _whatIsFloor);
            if (!hit) continue;
            return true;
        }

        return false;
    }
}