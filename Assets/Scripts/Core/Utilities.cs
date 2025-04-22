using System.Linq;
using UnityEngine;

public static class Utilities
{
    public static void FlipX(this Transform tr, bool flipX)
    {
        tr.localScale = new Vector3(Mathf.Abs(tr.localScale.x) * (flipX ? -1f : 1f), tr.localScale.y, tr.localScale.z);
    }

    public static RayCollision ToRayCollision(this Vector2[] origins, Vector2 direction, LayerMask mask)
    {
        return new RayCollision(origins.Select(origin => new Ray2D(origin, direction)).ToArray(), mask);
    }
}