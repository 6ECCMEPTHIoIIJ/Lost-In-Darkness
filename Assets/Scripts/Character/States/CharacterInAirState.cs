public class CharacterInAirState : CharacterState
{
    public CharacterCollisionsDetector Collisions { get; set; }
    public CharacterInAirPhysicsManager InAirPhysics { get; set; }

    public override void OnUpdate()
    {
    }

    public override void OnFixedUpdate()
    {
        if (!IsActive) return;
        
        InAirPhysics.OnFall();
        if (Collisions.IsGrounded)
        {
            StateManager.OnSwitchState(CharacterStates.Idle);
        }
    }
}