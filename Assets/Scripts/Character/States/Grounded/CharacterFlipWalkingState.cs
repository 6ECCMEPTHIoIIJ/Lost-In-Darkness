using UnityEngine;

public class CharacterFlipWalkingState : CharacterGroundedState
{
    private static readonly int FlipWalkingAnim = Animator.StringToHash("FlipWalking");

    private float _movementSpeed;
    private float _flipDuration;
    private float _enterTime;

    private Animator _anim;
    private InputManager _input;
    private Transform _transform;
    private Rigidbody2D _rb;

    public override void OnSetData(object data)
    {
        base.OnSetData(data);
        (_transform, _movementSpeed, _flipDuration, _anim, _input, _rb) = (Data)data;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(FlipWalkingAnim, true);
        _enterTime = Time.fixedTime;
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(FlipWalkingAnim, false);
        _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        _rb.linearVelocityX = _input.MovementDirection * _movementSpeed;
        if (Time.fixedTime - _enterTime > _flipDuration)
        {
            SwitchState(
                _input.MovementDirection == 0
                    ? CharacterStates.Idle
                    : CharacterStates.Walking
            );
        }
    }

    public new class Data : CharacterGroundedState.Data
    {
        public float MovementSpeed;
        public float FlipDuration;
        public Animator Anim;
        public Rigidbody2D Rb;

        public void Deconstruct(out Transform transform, out float movementSpeed, out float flipDuration,
            out Animator anim, out InputManager input, out Rigidbody2D rb)
        {
            transform = Transform;
            movementSpeed = MovementSpeed;
            flipDuration = FlipDuration;
            anim = Anim;
            input = Input;
            rb = Rb;
        }
    }
}