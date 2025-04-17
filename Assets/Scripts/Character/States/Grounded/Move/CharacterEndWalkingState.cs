using UnityEngine;

public class CharacterEndWalkingState : CharacterGroundedMoveState
{
    private static readonly int EndWalkingAnim = Animator.StringToHash("EndWalking");

    private float _endWalkingDuration;
    private float _enterTime;

    private Animator _anim;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_endWalkingDuration, _anim, _rb) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(EndWalkingAnim, true);
        _rb.linearVelocityX = 0f;
        _enterTime = Time.fixedTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(EndWalkingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Time.fixedTime - _enterTime > _endWalkingDuration)
        {
            SwitchState(CharacterStates.Idle);
        }
    }

    public new class Data : CharacterGroundedMoveState.Data
    {
        public float EndWalkingDuration;
        public Animator Anim;
        public Rigidbody2D Rb;

        public void Deconstruct(out float endWalkingDuration, out Animator anim, out Rigidbody2D rb)
        {
            endWalkingDuration = EndWalkingDuration;
            anim = Anim;
            rb = Rb;
        }
    }
}