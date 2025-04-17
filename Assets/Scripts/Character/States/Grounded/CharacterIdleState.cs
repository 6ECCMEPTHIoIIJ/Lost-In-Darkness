using UnityEngine;

public class CharacterIdleState : CharacterGroundedState
{
    private static readonly int IdleAnim = Animator.StringToHash("Idle");

    private float _inactionDuration;
    private float _enterTime;

    private InputManager _input;
    private Animator _anim;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_input, _inactionDuration, _anim, _rb) = (Data)data;
    }

    
    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(IdleAnim, true);
        _rb.linearVelocityX = 0f;
        _enterTime = Time.fixedTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(IdleAnim, false);
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (_input.IsJumping)
        {
            SwitchState(CharacterStates.Jumping);
        }
        if (_input.MovementDirection != 0)
        {
            SwitchState(CharacterStates.BeginWalking);
        }
        else if (Time.fixedTime - _enterTime > _inactionDuration)
        {
            SwitchState(CharacterStates.Scared);
        }
    }

    public new class Data : CharacterGroundedMoveState.Data
    {
        public float InactionDuration;
        public Animator Anim;
        public Rigidbody2D Rb;

        public void Deconstruct(out InputManager input, out float inactionDuration, out Animator anim, out Rigidbody2D rb)
        {
            input = Input;
            inactionDuration = InactionDuration;
            anim = Anim;
            rb = Rb;
        }
    }
}