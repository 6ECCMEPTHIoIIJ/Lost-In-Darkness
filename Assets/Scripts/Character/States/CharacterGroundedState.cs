public abstract class CharacterGroundedState : CharacterState
{
    public CharacterCollisionsDetector Collisions { get; set; }
    public CharacterInAirPhysicsManager InAirPhysics { get; set; }

    public override void OnEnter()
    {
        base.OnEnter();
        InAirPhysics.OnLand();
    }

    public override void OnFixedUpdate()
    {
        if (!IsActive) return;
        
        if (!Collisions.IsGrounded)
        {
            StateManager.OnSwitchState(CharacterStates.InAir);
        }
    }
}