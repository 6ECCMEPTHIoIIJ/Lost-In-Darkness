using UnityEngine;

public abstract class CharacterGroundedState : CharacterState
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (!Controller.Collisions.IsGrounded)
        {
            StateManager.OnSwitchState(CharacterStates.InAir);
        }
    }
}