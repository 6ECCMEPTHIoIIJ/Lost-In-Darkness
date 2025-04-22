using UnityEngine;

public class CharacterScaredState : CharacterGroundedState
{
    private static readonly int ScaredAnim = Animator.StringToHash("Scared");

    private FixedTimer _scaring;
    private Animator _anim;

    protected override CharacterStates Id => CharacterStates.Scared;

    public override void OnSetData(in CharacterStateData data)
    {
        base.OnSetData(in data);
        _anim = data.anim;
        _scaring = new FixedTimer(data.scareDuration);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _anim.SetTrigger(ScaredAnim);
        _scaring.Start();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!Active) return;

        if (!_scaring)
        {
            SwitchState(CharacterStates.Idle);
        }
    }
}