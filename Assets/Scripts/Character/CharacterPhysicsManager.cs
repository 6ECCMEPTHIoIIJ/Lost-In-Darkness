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

    private float _beginWalkingTime;
    private float _finishWalkingTime;
    private float _walkingDirection;

    public bool IsGrounded { get; set; }
    public bool IsJumping { get; set; }
    public float XVelocity { get; set; }
    public float YVelocity { get; set; }

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
        _beginWalkingTime = Time.fixedTime;
    }

    public void OnFinishWalking()
    {
        _finishWalkingTime = Time.fixedTime;
    }

    public void OnWalk(float walkingDirection)
    {
        _walkingDirection = walkingDirection;
        if (_beginWalkingTime == 0 || _beginWalkingTime < _finishWalkingTime) return;

        var walkingVelocity = _walkingDirection * walkingSpeed;
        var currentAccelerationTime = Time.fixedTime - _beginWalkingTime;
        if (currentAccelerationTime > accelerationDuration)
        {
            var currentWalkingTime = currentAccelerationTime - accelerationDuration;
            _rigidbody.linearVelocityX =
                walkingCurve.Evaluate(currentWalkingTime / walkingDuration)
                * walkingVelocity;
        }
        else
        {
            _rigidbody.linearVelocityX =
                accelerationCurve.Evaluate(currentAccelerationTime / accelerationDuration) 
                * walkingVelocity;
        }
    }

    public void OnIdling()
    {
        var walkingVelocity = _walkingDirection * walkingSpeed;
        var currentDecelerationTime = Time.fixedTime - _finishWalkingTime;
        if (currentDecelerationTime > decelerationDuration)
        {
            _rigidbody.linearVelocityX = 0;
        }
        else
        {
            _rigidbody.linearVelocityX =
                decelerationCurve.Evaluate(currentDecelerationTime / decelerationDuration) * walkingVelocity;
        }
    }
}