using System;
using UnityEngine;

public class CharacterFallingState : CharacterInAirState
{
    private static readonly int FallingAnim = Animator.StringToHash("Falling");

    private float _gravity;
    private float _maxFallSpeed;

    private Rigidbody2D _rb;
    private Animator _anim;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_rb, _gravity, _maxFallSpeed, _anim) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(FallingAnim, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(FallingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        _rb.linearVelocityY = Mathf.Max(-_maxFallSpeed, _rb.linearVelocityY - _gravity);
    }


    public new class Data : CharacterInAirState.Data
    {
        public float Gravity;
        public float MaxFallSpeed;

        public Animator Anim;

        public void Deconstruct(out Rigidbody2D rb, out float gravity, out float maxFallSpeed, out Animator anim)
        {
            rb = Rb;
            gravity = Gravity;
            maxFallSpeed = MaxFallSpeed;
            anim = Anim;
        }
    }
}