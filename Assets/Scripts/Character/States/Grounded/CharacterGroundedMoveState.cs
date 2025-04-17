using UnityEngine;

public abstract class CharacterGroundedMoveState : CharacterGroundedState
{
    private InputManager _input;
    private Transform _transform;

    private bool _flipX;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_input, _transform) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _flipX = _transform.localScale.x < 0f;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (_input.IsJumping)
        {
            SwitchState(CharacterStates.Jumping);
        }
        else if (_input.FlipX != _flipX)
        {
            SwitchState(CharacterStates.FlipWalking);
        }
    }

    public new abstract class Data : CharacterGroundedState.Data
    {
        public InputManager Input;

        public void Deconstruct(out InputManager input, out Transform transform)
        {
            input = Input;
            transform = Transform;
        }
    }
}