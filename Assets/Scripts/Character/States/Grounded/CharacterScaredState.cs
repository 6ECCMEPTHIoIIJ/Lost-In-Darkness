public class CharacterScaredState : CharacterState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        Controller.Effects.DaredEvent += () => StateManager.OnSwitchState(CharacterStates.Idle);
    }
    
    public override void OnEnter()
    {
        base.OnEnter();
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.Scared);
        Controller.Effects.OnBeginScaring();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;
        
        Controller.Effects.OnScare();
    }
}