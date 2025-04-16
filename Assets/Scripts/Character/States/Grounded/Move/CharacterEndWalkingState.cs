using System;
using UnityEngine;

public class CharacterEndWalkingState : CharacterState
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
        Controller.Physics.MovementDirection = Controller.Input.MovementDirection;
        Controller.Physics.OnEndWalking();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Controller.Input.MovementDirection != 0)
        {
            StateManager.OnSwitchState(CharacterStates.BeginWalking);
        }
        else
        {
            Controller.Physics.OnEndWalking();
        }
    }
}