using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private bool _isStopping;
    private float _stopTime;

    [SerializeField] private float stopDelay = 0.1f;

    public float MovementDirection { get; private set; }
    public bool FlipX { get; private set; }

    public void OnMove(InputAction.CallbackContext context)
    {
        var movementDirection = context.ReadValue<float>();
        _isStopping = movementDirection == 0;
        if (!_isStopping)
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
        if (_isStopping && Time.fixedTime > _stopTime)
        {
            MovementDirection = 0;
        }
    }
}