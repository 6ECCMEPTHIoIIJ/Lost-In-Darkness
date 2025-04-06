using System;
using UnityEngine;

public class CharacterIdleState : CharacterGroundedState
{
    public CharacterInputManager Input { get; set; }
    public CharacterWalkingPhysicsManager WalkingPhysics { get; set; }
    public CharacterAnimationManager Animation { get; set; }

    public override void OnEnter()
    {
        base.OnEnter();
        Animation.SetBool(CharacterAnimations.Idle, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        Animation.SetBool(CharacterAnimations.Idle, false);
    }

    public override void OnUpdate()
    {
        if (WalkingPhysics.IsScared)
        {
            Animation.SetTrigger(CharacterAnimations.Scared);
        }
        else
        {
            Animation.ResetTrigger(CharacterAnimations.Scared);
        }
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Input.IsWalking && !WalkingPhysics.IsScared)
        {
            StateManager.OnSwitchState(CharacterStates.Walking);
        }
    }
}