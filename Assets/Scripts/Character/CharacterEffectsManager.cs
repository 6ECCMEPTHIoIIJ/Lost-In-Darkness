using System;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    private float _beginScareTime;
    private float _beginDareTime;

    [SerializeField] private float scareCooldown;
    [SerializeField] private float scareDuration;

    public event Action ScaredEvent;
    public event Action DaredEvent;

    public void OnBeginDaring()
    {
        _beginDareTime = Time.fixedTime;
    }
    
    public void OnDare()
    {
        if (Time.fixedTime - _beginDareTime > scareCooldown)
        {
            ScaredEvent?.Invoke();
        }
    }

    public void OnBeginScaring()
    {
        _beginScareTime = Time.fixedTime;
    }

    public void OnScare()
    {
        if (Time.fixedTime - _beginScareTime > scareDuration)
        {
            DaredEvent?.Invoke();
        }
    }
}