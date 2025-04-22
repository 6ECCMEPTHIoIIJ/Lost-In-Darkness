using UnityEngine;

public class CharacterEndRunState : CharacterGroundedState
{
    private static readonly int EndWalkingAnim = Animator.StringToHash("EndWalking");

    private FixedTimer _running;

    private Animator _anim;
    private Rigidbody2D _rb;

    protected override CharacterStates Id => CharacterStates.EndRun;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _rb = data.rb;
        _anim = data.anim;
        _running = new FixedTimer(data.endRunDuration);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetBool(EndWalkingAnim, true);
        _running.Start();
    }

    public override void OnExit()
    {
        base.OnExit();
        _anim.SetBool(EndWalkingAnim, false);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        if (!_running)
        {
            SwitchState(CharacterStates.Idle);
        }
    }
}