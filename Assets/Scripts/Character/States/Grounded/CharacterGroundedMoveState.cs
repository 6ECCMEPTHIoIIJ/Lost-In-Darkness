public class CharacterGroundedMoveState : CharacterState
{
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (!IsActive) return;

        if (Controller.Input.FlipX != Controller.Physics.FlipX)
        {
            StateManager.OnSwitchState(CharacterStates.FlipWalking);
        }
    }
}