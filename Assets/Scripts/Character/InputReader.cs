using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputReader : MonoBehaviour, IMoveXBrain, IJumpBrain
{
    [SerializeField] private FixedTimer holdingJump;

    public float MoveX { get; private set; }
    public bool FlipX { get; private set; }

    public bool Jump { get; private set; }
    public bool JumpHold { get; private set; }
    
    public void OnJumpPerformed()
    {
        Jump = false;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveX = context.ReadValue<float>();
        FlipX = MoveX < 0 || FlipX && MoveX == 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Jump = true;
            JumpHold = true;
            holdingJump.Start();
        }
        else if (context.canceled)
        {
            JumpHold = false;
        }
    }

    public void FixedUpdate()
    {
        Jump &= holdingJump;
    }
}