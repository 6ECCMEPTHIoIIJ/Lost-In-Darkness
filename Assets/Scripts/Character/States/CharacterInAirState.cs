using UnityEngine;

public class CharacterInAirState : CharacterState
{
    public override void OnEnter()
    {
        base.OnEnter();
        Controller.Animation.OnSwitchAnimation(CharacterAnimations.Falling);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        Controller.Physics.MovementDirection = Controller.Input.MovementDirection;
        Controller.Physics.OnFall();
        if (Controller.Collisions.IsGrounded)
        {
            Controller.Physics.OnLand(Controller.Collisions.GroundDetectedPosition);
            StateManager.OnSwitchState(CharacterStates.Idle);
        }
    }
}