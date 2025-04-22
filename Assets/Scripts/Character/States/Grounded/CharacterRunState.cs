using UnityEngine;

public class CharacterRunState : CharacterGroundedState
{
    private static readonly int WalkingAnim = Animator.StringToHash("Walking");

    private IMoveXBrain _moveBr;
    private Animator _anim;

    protected override CharacterStates Id => CharacterStates.Run;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _moveBr = data.input;
        _anim = data.anim;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(WalkingAnim, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(WalkingAnim, false);
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        if (_moveBr.MoveX == 0)
        {
            SwitchState(CharacterStates.EndRun);
        }
    }
}