using System;
using UnityEngine;

public class CharacterBeginWalkingState : CharacterGroundedMoveState
{
    private static readonly int BeginWalkingAnim = Animator.StringToHash("BeginWalking");

    private float _movementSpeed;
    private float _beginWalkingDelay;
    private float _beginWalkingDuration;
    private float _enterTime;
    private float _movementDirection;

    private InputManager _input;
    private Animator _anim;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_movementSpeed, _beginWalkingDelay, _beginWalkingDuration, _input, _anim, _rb) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(BeginWalkingAnim, true);
        _rb.linearVelocityX = 0f;
        _movementDirection = _input.MovementDirection;
        _enterTime = Time.fixedTime;
    }

    public override void OnExit(CharacterStates to)
    {
        base.OnExit(to);
        _anim.SetBool(BeginWalkingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Time.fixedTime - _enterTime > _beginWalkingDelay)
        {
            _rb.linearVelocityX = _movementDirection * _movementSpeed;
            if (Time.fixedTime - _enterTime > _beginWalkingDuration)
            {
                SwitchState(CharacterStates.Walking);
            }
        }
        else if (_input.MovementDirection == 0)
        {
            SwitchState(CharacterStates.EndWalking);
        }
    }

    public new class Data : CharacterGroundedMoveState.Data
    {
        public float MovementSpeed;
        public float BeginWalkingDelay;
        public float BeginWalkingDuration;
        public Animator Anim;
        public Rigidbody2D Rb;

        public void Deconstruct(out float movementSpeed, out float beginWalkingDelay, out float beginWalkingDuration,
            out InputManager input, out Animator anim, out Rigidbody2D rb)
        {
            input = Input;
            movementSpeed = MovementSpeed;
            beginWalkingDelay = BeginWalkingDelay;
            beginWalkingDuration = BeginWalkingDuration;
            anim = Anim;
            rb = Rb;
        }
    }
}