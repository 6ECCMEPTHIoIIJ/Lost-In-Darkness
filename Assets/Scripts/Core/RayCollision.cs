using System;
using System.Linq;
using UnityEngine;

[Serializable]
public struct RayCollision
{
    [SerializeField] private Rect[] detects;
    [SerializeField] private LayerMask whatIsTarget;

    public RayCollision(Ray2D[] detects = null, LayerMask whatIsTarget = default)
    {
        this.detects = detects?.Select(ray => new Rect(ray.origin, ray.direction)).ToArray();
        this.whatIsTarget = whatIsTarget;
    }

    public bool Colliding(Vector2 position)
    {
        var mask = whatIsTarget;
        return detects.Select(detect =>
            Physics2D.Raycast(
                detect.position + position,
                detect.size.normalized,
                0.1f,
                mask
            )).Any(hit => hit);
    }
}