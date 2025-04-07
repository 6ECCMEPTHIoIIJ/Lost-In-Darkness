public abstract class CharacterGroundedState : CharacterState
{
    public CharacterCollisionsDetector Collisions { get; set; }
    public override void OnFixedUpdate()
    {
        if (!IsActive) return;
        
        if (!Collisions.IsGrounded)
        {
            StateManager.OnSwitchState(CharacterStates.InAir);
        }
    }
}