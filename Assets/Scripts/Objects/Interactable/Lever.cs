using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ParticleSystem))]
public class Lever : MonoBehaviour, InteractableObject
{
    [SerializeField] private SwitchableObject[] objectsToSwitch;

    private static readonly int SwitchAnim = Animator.StringToHash("Switch");

    private Animator _anim;
    private ParticleSystem _pSystem;

    private bool position = false;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _pSystem = GetComponent<ParticleSystem>();
    }

    public void Interact(bool isActingFromLeftToRight)
    {
        if (isActingFromLeftToRight && !position || !isActingFromLeftToRight && position)
        {
            foreach (var objectToSwitch in objectsToSwitch)
                objectToSwitch.Switch();

            position = !position;
            _anim.SetTrigger(SwitchAnim);
        }
    }

    public void Dust()
    {
        _pSystem.Play();
    }

    public void Mirror()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
