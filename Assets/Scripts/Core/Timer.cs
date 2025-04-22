using System;
using UnityEngine;

[Serializable]
public struct Timer
{
    private float _startTime;

    [SerializeField] private float duration;

    public Timer(float duration)
    {
        _startTime = 0f;
        this.duration = duration;
    }

    public void Start()
    {
        _startTime = Time.time;
    }

    public bool Check()
    {
        return Time.time - _startTime < duration;

    }
}

[Serializable]
public struct FixedTimer
{
    private float _startTime;

    [SerializeField] private float duration;

    public FixedTimer(float duration)
    {
        _startTime = 0f;
        this.duration = duration;
    }

    public void Start()
    {
        _startTime = Time.fixedTime;
    }

    public bool Check()
    {
        return Time.time - _startTime < duration;

    }
}