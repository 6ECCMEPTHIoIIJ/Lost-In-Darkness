using System;
using UnityEngine;

public class CharacterEndWalkingState : CharacterWalkingBaseState
{
    public override void OnInitialize()
    {
        base.OnInitialize();
        Controller.Physics.IdleEvent += () => StateManager.OnSwitchState(CharacterStates.Idle);
    }
    
    public override void OnEnter()
    {
        base.OnEnter();

        var endWalkingOffset = Controller.Animation.CurrentAnimationId switch
        {
            CharacterAnimations.Walking => Controller.Animation.EndWalkingAnimation.length * 0.6f,
            CharacterAnimations.BeginWalking => Controller.Animation.CurrentAnimationDurationLeft,
            _ => 0f,
        };
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.EndWalking, endWalkingOffset);
        Controller.Physics.WalkingDirection = Controller.Input.WalkingDirection;
        Controller.Physics.OnEndWalking();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Controller.Input.WalkingDirection != 0)
        {
            StateManager.OnSwitchState(CharacterStates.BeginWalking);
        }
        else
        {
            Controller.Physics.OnEndWalking();
        }
    }
}