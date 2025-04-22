using UnityEngine;

public class CharacterFallingState : CharacterInAirState
{
    private static readonly int FallingAnim = Animator.StringToHash("Falling");

    private float _maxFallSpeed;

    private Rigidbody2D _rb;
    private Animator _anim;

    protected override CharacterStates Id => CharacterStates.Falling;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _rb = data.rb;
        _anim = data.anim;
        _maxFallSpeed = data.maxFallSpeed;
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
        if (!Active) return;

        _rb.linearVelocityY =
            Mathf.Max(-_maxFallSpeed, _rb.linearVelocityY + Physics2D.gravity.y * Time.fixedDeltaTime);
    }
}