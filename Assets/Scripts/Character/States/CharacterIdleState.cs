public class CharacterIdleState : CharacterGroundedState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        Controller.Effects.ScaredEvent += () => StateManager.OnSwitchState(CharacterStates.Scared);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.Idle);
        Controller.Effects.OnBeginDaring();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Controller.Input.IsWalking)
        {
            StateManager.OnSwitchState(CharacterStates.BeginWalking);
        }
        else
        {
            Controller.Effects.OnDare();
        }
    }
}