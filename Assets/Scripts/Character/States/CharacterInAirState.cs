using UnityEngine;

public abstract class CharacterInAirState : State<CharacterStates, CharacterStateData>
{
    private float _inAirSpeed;
    private Rect[] _floorDetects;
    private LayerMask _whatIsFloor;

    private Transform _tr;
    private IMoveXBrain _moveBr;
    private Rigidbody2D _rb;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _tr = data.tr;
        _moveBr = data.input;
        _rb = data.rb;
        _inAirSpeed = data.inAirSpeed;
        _floorDetects = data.floorDetects;
        _whatIsFloor = data.whatIsGround;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        _rb.linearVelocityX = _moveBr.MoveX * _inAirSpeed;
        if (_moveBr.MoveX > 0 != _tr.localScale.x > 0 && _moveBr.MoveX != 0)
        {
            _tr.localScale = new Vector3(
                Mathf.Sign(_moveBr.MoveX) * Mathf.Abs(_tr.localScale.x),
                _tr.localScale.y, _tr.localScale.z);
        }

        if (Id == CharacterStates.Jumping) return;
        if (IsTouchingFloor(out var groundPoint))
        {
            _tr.position = new Vector2(_tr.position.x, groundPoint.y);
            _rb.linearVelocityY = 0;
            SwitchState(_moveBr.MoveX == 0
                ? CharacterStates.Idle
                : CharacterStates.Run
            );
        }
    }

    private bool IsTouchingFloor(out Vector2 groundPoint)
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
            groundPoint = hit.point;
            return true;
        }

        groundPoint = default;
        return false;
    }
}