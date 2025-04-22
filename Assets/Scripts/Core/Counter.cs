using System;
using UnityEngine;

[Serializable]
public struct Counter
{
    private int _count;

    [SerializeField] private int limit;

    public Counter(int limit = 0)
    {
        this.limit = limit;
        _count = 0;
    }

    public void Count(bool condition)
    {
        _count = condition ? _count + 1 : 0;
    }

    public bool Check()
    {
        return _count >= limit;
    }
}