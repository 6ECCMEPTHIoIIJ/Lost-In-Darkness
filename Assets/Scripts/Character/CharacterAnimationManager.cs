using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationManager : MonoBehaviour
{
    private Animator _animator;
    private CharacterAnimationKeyValue[] _animations;

    private float _endAnimationTime;

    [field: SerializeField] public AnimationClip IdleAnimation { get; set; }
    [field: SerializeField] public AnimationClip ScaredAnimation { get; set; }
    [field: SerializeField] public AnimationClip BeginWalkingAnimation { get; set; }
    [field: SerializeField] public AnimationClip WalkingAnimation { get; set; }
    [field: SerializeField] public AnimationClip EndWalkingAnimation { get; set; }
    [field: SerializeField] public AnimationClip FlipAnimation { get; set; }
    [field: SerializeField] public AnimationClip JumpingAnimation { get; set; }
    [field: SerializeField] public AnimationClip FallingAnimation { get; set; }

    public CharacterAnimations CurrentAnimationId { get; private set; } = CharacterAnimations.None;
    public float CurrentAnimationDurationLeft => Mathf.Max(0f, _endAnimationTime - Time.fixedTime);

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        _animations = new SortedDictionary<CharacterAnimations, CharacterAnimationKeyValue>
        {
            [CharacterAnimations.Idle] = new()
            {
                AnimationHash = Animator.StringToHash("Idle"),
                Animation = IdleAnimation
            },
            [CharacterAnimations.Scared] = new()
            {
                AnimationHash = Animator.StringToHash("Scared"),
                Animation = ScaredAnimation
            },
            [CharacterAnimations.BeginWalking] = new()
            {
                AnimationHash = Animator.StringToHash("Begin Walking"),
                Animation = BeginWalkingAnimation
            },
            [CharacterAnimations.Walking] = new()
            {
                AnimationHash = Animator.StringToHash("Walking"),
                Animation = WalkingAnimation
            },
            [CharacterAnimations.EndWalking] = new()
            {
                AnimationHash = Animator.StringToHash("End Walking"),
                Animation = EndWalkingAnimation
            },
            [CharacterAnimations.Jumping] = new()
            {
                AnimationHash = Animator.StringToHash("Jumping"),
                Animation = JumpingAnimation
            },
            [CharacterAnimations.Falling] = new()
            {
                AnimationHash = Animator.StringToHash("Falling"),
                Animation = FallingAnimation
            },
            [CharacterAnimations.Flip] = new()
            {
                AnimationHash = Animator.StringToHash("Flip"),
                Animation = FlipAnimation
            },
        }.Values.ToArray();

        OnSwitchAnimation(CharacterAnimations.Idle);
    }

    public void OnSwitchAnimation(CharacterAnimations animationId, float offset = 0f, float speed = 1f)
    {
        if (CurrentAnimationId == animationId) return;

        var currentAnimation = _animations[(int)animationId];
        _animator.speed = speed;
        _animator.PlayInFixedTime(currentAnimation.AnimationHash, -1, offset);
        CurrentAnimationId = animationId;
        _endAnimationTime = Time.fixedTime + currentAnimation.Animation.length;
    }
}

public struct CharacterAnimationKeyValue
{
    public int AnimationHash { get; set; }
    public AnimationClip Animation { get; set; }
}

public enum CharacterAnimations
{
    None = -1,
    Idle = 0,
    Scared,
    BeginWalking,
    Walking,
    EndWalking,
    Jumping,
    Falling,
    Flip,
    Count,
}