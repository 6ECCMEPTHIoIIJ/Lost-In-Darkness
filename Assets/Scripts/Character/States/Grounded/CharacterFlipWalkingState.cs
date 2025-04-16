public class CharacterFlipWalkingState : CharacterState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        Controller.Physics.BeginWalkingEvent += () => StateManager.OnSwitchState(CharacterStates.BeginWalking);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.Flip);
        Controller.Physics.FlipX = Controller.Input.FlipX;
        Controller.Physics.OnBeginFlip();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        Controller.Physics.OnFlip();
    }
}