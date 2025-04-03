using System.Collections;
using UnityEngine;

public class CharacterPhysicsManager : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float accelerationDuration;
    [SerializeField] private AnimationCurve accelerationCurve;
    [SerializeField] private float walkingDuration;
    [SerializeField] private AnimationCurve walkingCurve;
    [SerializeField] private float decelerationDuration;
    [SerializeField] private AnimationCurve decelerationCurve;

    private float _beginAccelerationTime;
    private float _beginWalkingTime;
    private float _bedingDecelerationTime;

    public float WalkingDirection { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsJumping { get; set; }
    public float XVelocity { get; set; }
    public float YVelocity { get; set; }

    public CharacterWalkingStates WalkingState { get; private set; }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        XVelocity = _rigidbody.linearVelocityX;
        YVelocity = _rigidbody.linearVelocityY;
    }

    public void OnBeginWalking()
    {
        WalkingState = CharacterWalkingStates.Accelerating;
        _beginAccelerationTime = Time.fixedTime;
        StopAllCoroutines();

        var currentDecelerationDuration = Time.fixedTime - _bedingDecelerationTime;
        var offset = currentDecelerationDuration < decelerationDuration
            ? (decelerationDuration - currentDecelerationDuration) / decelerationDuration
            : 0;
        StartCoroutine(Accelerate(offset));
    }

    public void OnFinishWalking()
    {
        WalkingState = CharacterWalkingStates.Decelerating;
        _bedingDecelerationTime = Time.fixedTime;
        StopAllCoroutines();

        var currentAccelerationDuration = Time.fixedTime - _beginAccelerationTime;
        var offset = currentAccelerationDuration < accelerationDuration
            ? (accelerationDuration - currentAccelerationDuration) / accelerationDuration
            : 0;
        StartCoroutine(Decelerate(offset));
    }

    private IEnumerator Accelerate(float offset)
    {
        for (var currentDuration = offset * accelerationDuration;
             currentDuration < accelerationDuration;
             currentDuration = Time.fixedTime - _beginAccelerationTime)
        {
            var walkingVelocity = WalkingDirection * walkingSpeed;
            _rigidbody.linearVelocityX =
                accelerationCurve.Evaluate(currentDuration / accelerationDuration)
                * walkingVelocity;

            yield return new WaitForFixedUpdate();
        }

        _beginWalkingTime = Time.fixedTime;
        WalkingState = CharacterWalkingStates.Walking;
    }

    private IEnumerator Decelerate(float offset)
    {
        for (var currentDuration = offset * decelerationDuration;
             currentDuration < decelerationDuration;
             currentDuration = Time.fixedTime - _bedingDecelerationTime)
        {
            var walkingVelocity = WalkingDirection * walkingSpeed;
            _rigidbody.linearVelocityX =
                decelerationCurve.Evaluate(currentDuration / decelerationDuration)
                * walkingVelocity;

            yield return new WaitForFixedUpdate();
        }

        WalkingState = CharacterWalkingStates.Idle;
    }

    public void OnWalk()
    {
        var currentDuration = Time.fixedTime - _beginWalkingTime;
        var walkingVelocity = WalkingDirection * walkingSpeed;
        _rigidbody.linearVelocityX =
            walkingCurve.Evaluate(currentDuration / walkingDuration)
            * walkingVelocity;
    }

    public void OnIdling()
    {
        _rigidbody.linearVelocityX = 0;
    }
}

public enum CharacterWalkingStates
{
    Idle = 0,
    Accelerating,
    Walking,
    Decelerating,
    Count,
}