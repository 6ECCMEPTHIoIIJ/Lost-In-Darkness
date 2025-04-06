using System;
using UnityEngine;

public class CharacterWalkingState : CharacterGroundedState
{
    public CharacterInputManager Input { get; set; }
    public CharacterWalkingPhysicsManager WalkingPhysics { get; set; }
    public CharacterAnimationManager Animation { get; set; }

    public override void OnEnter()
    {
        base.OnEnter();
        WalkingPhysics.WalkingDirection = Input.WalkingDirection;
        WalkingPhysics.OnWalk();
    }

    public override void OnUpdate()
    {
        Animation.SetBool(CharacterAnimations.Walking, Input.IsWalking);
        if (WalkingPhysics.IsFlipping)
        {
            Animation.SetTrigger(CharacterAnimations.Flipped);
        }
        else
        {
            Animation.ResetTrigger(CharacterAnimations.Flipped);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        WalkingPhysics.WalkingDirection = Input.WalkingDirection;
        if (WalkingPhysics.IsWalking)
        {
            WalkingPhysics.OnWalk();
        }
        else
        {
            StateManager.OnSwitchState(CharacterStates.Idle);
        }
    }
}