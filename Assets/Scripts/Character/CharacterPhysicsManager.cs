using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterPhysicsManager : MonoBehaviour
{
    private Transform _transform;
    private Rigidbody2D _rigidbody;
    private Light2D _light;

    private float _beginWalkingTime;
    private float _endWalkingTime;
    private float _beginFlipTime;

    [SerializeField] private float walkingSpeed;
    [SerializeField] private float slowWalkingSpeed;
    [SerializeField] private float beginWalkingDuration;
    [SerializeField] private float beginSlowWalkingDuration;
    [SerializeField] private float endWalkingDuration;
    [SerializeField] private float endSlowWalkingDuration;
    [SerializeField] private float flipDuration;

    [SerializeField] private float inAirSpeed;
    [SerializeField] private float fallingSpeed;
    [SerializeField] private float gravity;

    public event Action BeginWalkingEvent;
    public event Action EndWalkingEvent;
    public event Action WalkingEvent;
    public event Action IdleEvent;

    public float MovementDirection { get; set; }
    public bool FlipX { get; set; }

    private void Awake()
    {
        _transform = transform;
        _rigidbody = GetComponent<Rigidbody2D>();
        _light = GetComponentInChildren<Light2D>();
    }

    public void OnBeginFlip()
    {
        _beginFlipTime = Time.fixedTime;
        _rigidbody.linearVelocityX = 0f;
    }

    public void OnFlip()
    {
        var currentDuration = Time.fixedTime - _beginFlipTime;
        if (currentDuration > flipDuration)
        {
            _transform.localScale =
                new Vector3(Mathf.Abs(_transform.localScale.x) * (FlipX ? -1f : 1f), _transform.localScale.y,
                    _transform.localScale.z);
            BeginWalkingEvent?.Invoke();
        }
    }

    public void OnBeginWalking()
    {
        if (_beginWalkingTime <= _endWalkingTime)
        {
            _beginWalkingTime = Time.fixedTime;
            return;
        }

        var currentDuration = Time.fixedTime - _beginWalkingTime;
        if (currentDuration > beginWalkingDuration)
        {
            WalkingEvent?.Invoke();
        }
        else
        {
            _rigidbody.linearVelocityX = currentDuration < beginSlowWalkingDuration
                ? 0
                : MovementDirection * slowWalkingSpeed;
        }
    }

    public void OnWalk()
    {
        if (MovementDirection == 0)
        {
            EndWalkingEvent?.Invoke();
        }
        else
        {
            _rigidbody.linearVelocityX = MovementDirection * walkingSpeed;
        }
    }

    public void OnEndWalking()
    {
        if (_beginWalkingTime >= _endWalkingTime)
        {
            _endWalkingTime = Time.fixedTime;
            return;
        }

        var currentDuration = Time.fixedTime - _endWalkingTime;
        if (currentDuration > endWalkingDuration)
        {
            IdleEvent?.Invoke();
        }
        else
        {
            _rigidbody.linearVelocityX = currentDuration < endSlowWalkingDuration
                ? MovementDirection * slowWalkingSpeed
                : 0;
        }
    }

    public void OnFall()
    {
        _rigidbody.linearVelocityX = MovementDirection * inAirSpeed;
        _rigidbody.linearVelocityY =
            Mathf.Max(-fallingSpeed, _rigidbody.linearVelocityY - gravity * Time.fixedDeltaTime);
    }

    public void OnLand(Vector2 groundDetectedPosition)
    {
        _rigidbody.position = new Vector2(_rigidbody.position.x, groundDetectedPosition.y);
        _rigidbody.linearVelocityY = 0;
    }
}