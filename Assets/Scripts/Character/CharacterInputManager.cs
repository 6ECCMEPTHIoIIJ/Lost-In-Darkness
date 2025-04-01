using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterInputManager : MonoBehaviour
{
    private float _finishWalkingTime;

    [SerializeField] private float walkingDelay;
    public float WalkingDirection { get; private set; }
    public bool FlipX => WalkingDirection < 0;
    public float WalkingTime { get; private set; }
    public bool IsWalking => WalkingTime > walkingDelay;

    public void OnWalk(InputAction.CallbackContext context)
    {
        WalkingDirection = context.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        if (WalkingDirection == 0)
        {
            _finishWalkingTime = Time.fixedTime;
            WalkingTime = 0;
        }
        else
        {
            WalkingTime = Time.fixedTime - _finishWalkingTime;
        }
    }
}