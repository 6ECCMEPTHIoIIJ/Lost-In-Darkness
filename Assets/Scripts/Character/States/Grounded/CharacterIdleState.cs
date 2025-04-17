using UnityEngine;

public class CharacterIdleState : CharacterGroundedState
{
    private static readonly int IdleAnim = Animator.StringToHash("Idle");

    private float _inactionDuration;
    private float _enterTime;

    private InputManager _input;
    private Animator _anim;


    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(IdleAnim, true);
        _enterTime = Time.fixedTime;
    }

    public override void OnExit(CharacterStates to)
    {
        base.OnExit(to);
        _anim.SetBool(IdleAnim, false);
    }

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_inactionDuration, _input, _anim) = (Data)data;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

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

        public void Deconstruct(out float inactionDuration, out InputManager input, out Animator anim)
        {
            inactionDuration = InactionDuration;
            input = Input;
            anim = Anim;
        }
    }
}