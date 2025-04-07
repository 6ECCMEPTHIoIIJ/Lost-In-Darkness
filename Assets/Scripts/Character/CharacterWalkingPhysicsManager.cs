using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterWalkingPhysicsManager : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _sprite;

    private bool _prevFlipX;
    private bool _flipX;

    private float _beginWalkingTime;
    private float _endWalkingTime;
    private float _beginScareTime;
    private float _endScareTime;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float accelerationDuration;
    [SerializeField] private float decelerationDuration;

    [SerializeField] private float fearlessnessDuration;
    [SerializeField] private float scareDuration;

    public float WalkingDirection { get; set; }
    public bool IsFlipping => _prevFlipX != _flipX;

    public bool IsWalking => _beginWalkingTime > _endWalkingTime ||
                             Time.fixedTime - _endWalkingTime <= decelerationDuration;

    public bool IsScared => _beginScareTime < Time.fixedTime && Time.fixedTime <= _endScareTime;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _sprite = GetComponent<SpriteRenderer>();
        
        _prevFlipX = _sprite.flipX;
        _flipX = _sprite.flipX;
    }

    public void FixedUpdate()
    {
        if (IsWalking)
        {
            _beginScareTime = 0;
            _endScareTime = 0;
        }
        else if (Time.fixedTime > _endScareTime)
        {
            _beginScareTime = Time.fixedTime + fearlessnessDuration * Random.Range(0.8f, 1.2f);
            _endScareTime = _beginScareTime + scareDuration;
        }
    }

    public void OnFlip()
    {
        _sprite.flipX = _flipX;
    }

    public void OnWalk()
    {
        _prevFlipX = _flipX;
        _flipX = WalkingDirection < 0 || WalkingDirection == 0 && _flipX;

        if (IsFlipping || WalkingDirection == 0 && _beginWalkingTime >= _endWalkingTime)
        {
            _endWalkingTime = Time.fixedTime;
        }
        else if (WalkingDirection != 0 && _beginWalkingTime <= _endWalkingTime)
        {
            _beginWalkingTime = Time.fixedTime;
        }

        if (_beginWalkingTime >= _endWalkingTime && Time.fixedTime - _beginWalkingTime > accelerationDuration)
        {
            var walkingVelocity = WalkingDirection * walkingSpeed;
            _rigidbody.linearVelocityX = walkingVelocity;
        }
        else
        {
            _rigidbody.linearVelocityX = 0;
        }
    }
}