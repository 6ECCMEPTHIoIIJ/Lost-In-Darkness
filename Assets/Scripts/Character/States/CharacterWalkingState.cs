using System;
using UnityEngine;

public class CharacterWalkingState : CharacterState
{
    private static readonly int WalkingAnimation = Animator.StringToHash("Walking");

    public CharacterInputManager Input { get; set; }
    public CharacterPhysicsManager Physics { get; set; }
    public Animator Animator { get; set; }
    public SpriteRenderer Sprite { get; set; }

    public override void OnEnter()
    {
        Physics.OnBeginWalking();
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        Physics.WalkingDirection = Input.WalkingDirection;
        Sprite.flipX = Input.FlipX || Input.WalkingDirection == 0 && Sprite.flipX;
        Animator.SetBool(WalkingAnimation, Input.IsWalking);

        switch (Physics.WalkingState)
        {
            case CharacterWalkingStates.Idle:
                OnIdling();
                break;
            case CharacterWalkingStates.Accelerating:
                OnAccelerate();
                break;
            case CharacterWalkingStates.Walking:
               OnWalk();
                break;
            case CharacterWalkingStates.Decelerating:
               OnDecelerate();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void OnIdling() => StateManager.OnSwitchState(CharacterStates.Idle);

    private void OnAccelerate()
    {
        if (!Input.IsWalking)
        {
            Physics.OnFinishWalking();
        }
    }

    private void OnWalk()
    {
        if (Input.IsWalking)
        {
            Physics.OnWalk();
        }
        else
        {
            Physics.OnFinishWalking();
        }
    }

    private void OnDecelerate()
    {
        if (Input.IsWalking)
        {
            Physics.OnBeginWalking();
        }
    }
}