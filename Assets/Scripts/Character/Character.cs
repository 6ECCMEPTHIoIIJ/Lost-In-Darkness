using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReader))]
public class Character : MonoBehaviour
{
    private readonly StateMachine<CharacterStates, CharacterStateData> _sm = new();
    [SerializeField] private CharacterStateData data;

    private void Awake()
    {
        data.rb = GetComponent<Rigidbody2D>();
        data.anim = GetComponent<Animator>();
        data.input = GetComponent<InputReader>();
        data.tr = transform;

        _sm.Initialize(new SortedDictionary<CharacterStates, IState<CharacterStates, CharacterStateData>>
        {
            [CharacterStates.Idle] = new CharacterIdleState(),
            [CharacterStates.Run] = new CharacterRunState(),
            [CharacterStates.StartRun] = new CharacterStartRunState(),
            [CharacterStates.EndRun] = new CharacterEndRunState(),
            [CharacterStates.Flip] = new CharacterFlipState(),
            [CharacterStates.Scared] = new CharacterScaredState(),
            [CharacterStates.Falling] = new CharacterFallingState(),
            [CharacterStates.Jumping] = new CharacterJumpingState(),
        });
    }

    private void Start()
    {
        _sm.SetData(data);
        _sm.OnSwitchState(CharacterStates.Idle);
    }

    private void Update()
    {
        _sm.Update();
    }

    private void FixedUpdate()
    {
        _sm.FixedUpdate();
    }

    private List<Vector3> _gJumpPoints;

    private void OnDrawGizmos()
    {
        foreach (var detectRay in data.ceilDetects)
        {
            Gizmos.DrawRay(transform.position + new Vector3(detectRay.x * transform.localScale.x, detectRay.y),
                new Vector3(detectRay.size.x, detectRay.size.y));
        }

        foreach (var detectRay in data.floorDetects)
        {
            Gizmos.DrawRay(transform.position + new Vector3(detectRay.x * transform.localScale.x, detectRay.y),
                new Vector3(detectRay.size.x, detectRay.size.y));
        }

        if (data.input == null || !data.input.Jump)
        {
            var velocityX = data.inAirSpeed * transform.localScale.x;
            var jumpDuration = Mathf.Sqrt(2f * data.jumpHeight / -Physics2D.gravity.y);
            _gJumpPoints = new List<Vector3>();
            for (var jumpTime = jumpDuration; jumpTime > 0; jumpTime -= Time.fixedDeltaTime)
            {
                _gJumpPoints.Add(transform.position +
                                 new Vector3(velocityX * (jumpDuration - jumpTime),
                                     data.jumpHeight + jumpTime * jumpTime * Physics2D.gravity.y / 2f));
            }

            for (var jumpTime = 0f; jumpTime < jumpDuration; jumpTime += Time.fixedDeltaTime)
            {
                _gJumpPoints.Add(transform.position +
                                 new Vector3(velocityX * (jumpDuration + jumpTime),
                                     data.jumpHeight + jumpTime * jumpTime * Physics2D.gravity.y / 2f));
            }

            jumpDuration /= Mathf.Sqrt(2f);
            for (var jumpTime = jumpDuration; jumpTime > 0; jumpTime -= Time.fixedDeltaTime)
            {
                _gJumpPoints.Add(transform.position +
                                 new Vector3(velocityX * (jumpDuration + jumpTime),
                                     data.jumpHeight / 2f + jumpTime * jumpTime * Physics2D.gravity.y / 2f));
            }

            for (var jumpTime = 0f; jumpTime < jumpDuration; jumpTime += Time.fixedDeltaTime)
            {
                _gJumpPoints.Add(transform.position +
                                 new Vector3(velocityX * (jumpDuration - jumpTime),
                                     data.jumpHeight / 2f + jumpTime * jumpTime * Physics2D.gravity.y / 2f));
            }
        }

        Gizmos.DrawLineStrip(_gJumpPoints.ToArray(), false);
    }
}

public enum CharacterStates
{
    Idle = 0,
    Scared,
    StartRun,
    Run,
    EndRun,
    Flip,
    Jumping,
    Falling,
    Count,
}

[Serializable]
public struct CharacterStateData
{
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Animator anim;
    [HideInInspector] public InputReader input;
    [HideInInspector] public Transform tr;

    [Header("In Air")] public float inAirSpeed;
    public float maxFallSpeed;
    public float jumpHeight;

    [Header("Run")] public float runSpeed;
    public float startRunDuration;
    public float endRunDuration;
    public float flipDuration;

    [Header("Idle")] public float waitDuration;
    public float scareDuration;

    [Header("Collisions")] public LayerMask whatIsGround;
    public Rect[] floorDetects;
    public Rect[] ceilDetects;
}