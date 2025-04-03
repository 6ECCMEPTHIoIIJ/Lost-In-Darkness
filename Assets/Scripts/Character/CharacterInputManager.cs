using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterInputManager : MonoBehaviour
{
    public float WalkingDirection { get; private set; }
    public bool IsWalking => WalkingDirection != 0;
    public bool FlipX => WalkingDirection < 0;
    public float WalkingTime { get; private set; }

    public void OnWalk(InputAction.CallbackContext context)
    {
        WalkingDirection = context.ReadValue<float>();
    }
}