using System;
using UnityEngine;

public class CharacterIdleState : CharacterState
{
    private static readonly int IdleAnimation = Animator.StringToHash("Idle");
    public CharacterInputManager Input { get; set; }

    public CharacterPhysicsManager Physics { get; set; }
    public Animator Animator { get; set; }
    public SpriteRenderer Sprite { get; set; }

    
    public override void OnEnter()
    {
        Animator.SetBool(IdleAnimation, true);
    }

    public override void OnExit()
    {
        Animator.SetBool(IdleAnimation, false);
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        Physics.OnIdling();
        Sprite.flipX = Input.FlipX || Input.WalkingDirection == 0 && Sprite.flipX;

        if (Input.IsWalking)
        {
            StateManager.OnSwitchState(CharacterStates.Walking);
            return;
        }
    }
}