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
    }

    public override void OnExit()
    {
    }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        Physics.OnWalk(Input.WalkingDirection);
        Animator.SetBool(WalkingAnimation, Input.IsWalking);
        Sprite.flipX = Input.FlipX || Input.WalkingDirection == 0 && Sprite.flipX;

        if (!Input.IsWalking)
        {
            StateManager.OnSwitchState(CharacterStates.Idle);
        }
    }
}