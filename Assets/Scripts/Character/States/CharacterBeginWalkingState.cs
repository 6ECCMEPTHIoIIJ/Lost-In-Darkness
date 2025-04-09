using System;

public class CharacterBeginWalkingState : CharacterWalkingBaseState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        Controller.Physics.WalkingEvent += () => StateManager.OnSwitchState(CharacterStates.Walking);
    }

    public override void OnEnter()
    {
        base.OnEnter();

        var beginWalkingOffset = Controller.Animation.CurrentAnimationId switch
        {
            CharacterAnimations.Flip => Controller.Animation.BeginWalkingAnimation.length * 0.2f,
            CharacterAnimations.EndWalking => Controller.Animation.CurrentAnimationDurationLeft,
            _ => 0f,
        };
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.BeginWalking, beginWalkingOffset);
        Controller.Physics.MovementDirection = Controller.Input.MovementDirection;
        Controller.Physics.OnBeginWalking();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Controller.Input.MovementDirection == 0)
        {
            StateManager.OnSwitchState(CharacterStates.EndWalking);
        }
        else
        {
            Controller.Physics.MovementDirection = Controller.Input.MovementDirection;
            Controller.Physics.OnBeginWalking();
        }
    }
}