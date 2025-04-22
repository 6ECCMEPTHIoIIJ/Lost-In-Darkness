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

    public static implicit operator bool(Timer timer)
    {
        return Time.time - timer._startTime < timer.duration;
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

    public static implicit operator bool(FixedTimer timer)
    {
        return Time.fixedTime - timer._startTime < timer.duration;
    }
}