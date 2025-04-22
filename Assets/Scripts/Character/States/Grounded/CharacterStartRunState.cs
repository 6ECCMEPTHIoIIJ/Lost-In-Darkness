using UnityEngine;

public class CharacterStartRunState : CharacterGroundedState
{
    private static readonly int BeginWalkingAnim = Animator.StringToHash("BeginWalking");

    private FixedTimer _waiting;

    private IMoveXBrain _moveBr;
    private Animator _anim;

    protected override CharacterStates Id => CharacterStates.StartRun;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _moveBr = data.input;
        _anim = data.anim;
        _waiting = new FixedTimer(data.startRunDuration);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(BeginWalkingAnim, true);
        _waiting.Start();
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(BeginWalkingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        if (!_waiting)
        {
            SwitchState(CharacterStates.Run);
        }
        else if (_moveBr.MoveX == 0)
        {
            SwitchState(CharacterStates.EndRun);
        }
    }
}