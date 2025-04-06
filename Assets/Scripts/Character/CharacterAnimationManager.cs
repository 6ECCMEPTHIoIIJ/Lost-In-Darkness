using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class CharacterAnimationManager : MonoBehaviour
{
    private Animator _animator;
    private int[] _animations;

    private bool[] _triggers;

    private void Awake()
    {
        _animations = new SortedDictionary<CharacterAnimations, int>
        {
            [CharacterAnimations.Idle] = Animator.StringToHash("Idle"),
            [CharacterAnimations.Scared] = Animator.StringToHash("Scared"),
            [CharacterAnimations.Walking] = Animator.StringToHash("Walking"),
            [CharacterAnimations.Jumping] = Animator.StringToHash("Jumping"),
            [CharacterAnimations.Falling] = Animator.StringToHash("Falling"),
            [CharacterAnimations.Flipped] = Animator.StringToHash("Flipped"),
        }.Values.ToArray();
        _triggers = new bool[_animations.Length];
    }

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetBool(CharacterAnimations anim, bool value) => _animator.SetBool(_animations[(int)anim], value);
    public void SetFloat(CharacterAnimations anim, float value) => _animator.SetFloat(_animations[(int)anim], value);
    public void SetInteger(CharacterAnimations anim, int value) => _animator.SetInteger(_animations[(int)anim], value);

    public void SetTrigger(CharacterAnimations anim)
    {
        if (_triggers[(int)anim]) return;
        _animator.SetTrigger(_animations[(int)anim]);
        _triggers[(int)anim] = true;
    }

    public void ResetTrigger(CharacterAnimations anim) => _triggers[(int)anim] = false;
}

public enum CharacterAnimations
{
    Idle = 0,
    Scared,
    Walking,
    Jumping,
    Falling,
    Flipped,
    Count,
}