using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterInputManager : MonoBehaviour
{
    private bool _isStoping;
    private bool _isStopping;
    private float _stopTime;

    [SerializeField] private float stopDelay;

    public float MovementDirection { get; private set; }
    public bool IsMoving => MovementDirection != 0;
    public bool FlipX { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        var movementDirection = context.ReadValue<float>();
        _isStoping = movementDirection == 0;
        if (!_isStoping)
        {
            MovementDirection = movementDirection;
            FlipX = movementDirection < 0;
        }
        else
        {
            _stopTime = Time.fixedTime + stopDelay;
        }
    }

    private void FixedUpdate()
    {
        if (_isStoping && Time.fixedTime > _stopTime)
        {
            MovementDirection = 0;
        }
    }
}