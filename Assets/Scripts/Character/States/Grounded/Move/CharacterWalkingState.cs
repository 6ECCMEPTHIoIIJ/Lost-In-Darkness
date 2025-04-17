using System;
using UnityEngine;

public class CharacterWalkingState : CharacterGroundedMoveState
{
    private static readonly int WalkingAnim = Animator.StringToHash("Walking");

    private float _movementSpeed;
    private float _movementDirection;

    private InputManager _input;
    private Animator _anim;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_movementSpeed, _input, _anim, _rb) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(WalkingAnim, true);
        _movementDirection = _input.MovementDirection;
    }

    public override void OnExit(CharacterStates to)
    {
        base.OnExit(to);
        _anim.SetBool(WalkingAnim, false);
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        _rb.linearVelocityX = _movementDirection * _movementSpeed;
        if (_input.MovementDirection == 0)
        {
            SwitchState(CharacterStates.EndWalking);
        }
    }

    public new class Data : CharacterGroundedMoveState.Data
    {
        public float MovementSpeed;
        public Animator Anim;
        public Rigidbody2D Rb;

        public void Deconstruct(out float movementSpeed, out InputManager input, out Animator anim, out Rigidbody2D rb)
        {
            input = Input;
            movementSpeed = MovementSpeed;
            anim = Anim;
            rb = Rb;
        }
    }
}