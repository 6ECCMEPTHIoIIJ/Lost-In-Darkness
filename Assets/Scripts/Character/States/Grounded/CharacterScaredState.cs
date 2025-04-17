using UnityEngine;

public class CharacterScaredState : CharacterGroundedState
{
    private static readonly int ScaredAnim = Animator.StringToHash("Scared");

    private float _scareDuration;
    private float _enterTime;

    private Animator _anim;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_scareDuration, _anim) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(ScaredAnim, true);
        _enterTime = Time.fixedTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(ScaredAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Time.fixedTime - _enterTime > _scareDuration)
        {
            SwitchState(CharacterStates.Idle);
        }
    }

    public new class Data : CharacterGroundedState.Data
    {
        public float ScareDuration;
        public Animator Anim;

        public void Deconstruct(out float scaredDuration, out Animator anim)
        {
            scaredDuration = ScareDuration;
            anim = Anim;
        }
    }
}