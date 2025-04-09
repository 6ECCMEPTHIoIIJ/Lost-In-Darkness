using System;

public class CharacterWalkingState : CharacterWalkingBaseState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        Controller.Physics.EndWalkingEvent += () => StateManager.OnSwitchState(CharacterStates.EndWalking);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.Walking);
        Controller.Physics.MovementDirection = Controller.Input.MovementDirection;
        Controller.Physics.OnWalk();
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
            Controller.Physics.OnWalk();
        }
    }
}