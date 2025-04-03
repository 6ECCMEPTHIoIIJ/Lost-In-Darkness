using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class CharacterInputManager : MonoBehaviour
{
    private bool _isWalking;
    private bool _isStopping;

    [SerializeField] private float stopDelay;

    public float WalkingDirection { get; private set; }
    public bool IsWalking => WalkingDirection != 0;
    public bool FlipX => WalkingDirection < 0;
    public float WalkingTime { get; private set; }

    public void OnWalk(InputAction.CallbackContext context)
    {
        var walkingDirection = context.ReadValue<float>();
        _isWalking = walkingDirection != 0;
        if (_isWalking)
        {
            WalkingDirection = walkingDirection;
            if (_isStopping)
            {
                _isStopping = false;
                StopAllCoroutines();
            }
        }
        else if (!_isStopping)
        {
            _isStopping = true;
            StartCoroutine(StopWalking());
        }
    }

    private IEnumerator StopWalking()
    {
        yield return new WaitForSeconds(stopDelay);
        if (!_isWalking)
        {
            WalkingDirection = 0;
        }
    }
}