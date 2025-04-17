using UnityEngine;

public class CharacterFlipWalkingState : CharacterGroundedState
{
    private static readonly int FlipWalkingAnim = Animator.StringToHash("FlipWalking");


    private float _flipDuration;
    private float _enterTime;

    private Animator _anim;
    private InputManager _input;
    private Transform _transform;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_flipDuration, _anim, _input, _transform, _rb) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(FlipWalkingAnim, true);
        _rb.linearVelocityX = 0f;
        _enterTime = Time.fixedTime;
    }

    public override void OnExit(CharacterStates to)
    {
        base.OnExit(to);
        _anim.SetBool(FlipWalkingAnim, false);
        _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Time.fixedTime - _enterTime > _flipDuration)
        {
            SwitchState(
                _input.MovementDirection == 0
                    ? CharacterStates.Idle
                    : CharacterStates.BeginWalking
            );
        }
    }

    public new class Data : CharacterGroundedState.Data
    {
        public float FlipDuration;
        public Animator Anim;
        public InputManager Input;
        public Rigidbody2D Rb;

        public void Deconstruct(out float flipDuration, out Animator anim, out InputManager input,
            out Transform transform, out Rigidbody2D rb)
        {
            flipDuration = FlipDuration;
            anim = Anim;
            input = Input;
            transform = Transform;
            rb = Rb;
        }
    }
}