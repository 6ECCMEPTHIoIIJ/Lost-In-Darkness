using UnityEngine;

public class CharacterFlipState : CharacterGroundedState
{
    private static readonly int FlipWalkingAnim = Animator.StringToHash("FlipWalking");

    private FixedTimer _flipping;

    private Animator _anim;
    private IMoveXBrain _moveBr;
    private Transform _tr;

    private float _flipX;

    protected override CharacterStates Id => CharacterStates.Flip;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _anim = data.anim;
        _moveBr = data.input;
        _tr = data.tr;
        _flipping = new FixedTimer(data.flipDuration);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetTrigger(FlipWalkingAnim);
        _flipX = Mathf.Sign(_moveBr.MoveX);
        _flipping.Start();
    }

    public override void OnExit()
    {
        base.OnExit();
        var scale = _tr.localScale;
        _tr.localScale = new Vector3(_flipX * Mathf.Abs(scale.x), scale.y, scale.z);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        if (!_flipping)
        {
            SwitchState(_moveBr.MoveX == 0 ? CharacterStates.Idle : CharacterStates.Run);
        }
    }
}