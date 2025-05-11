using System;
using UnityEngine;

public static class UnityEx
{
    public static void FlipX(this Transform tr, bool flipX)
    {
        tr.localScale = new Vector3(Mathf.Abs(tr.localScale.x) * (flipX ? -1f : 1f), tr.localScale.y, tr.localScale.z);
    }

    public static void FromBounds(this BoxCollider2D collider, Rect bounds)
    {
        collider.size = bounds.size;
        collider.offset = bounds.center;
    }

    public static bool CollideAtX(this BoxCollider2D collider, RaycastHit2D[] hits, int x, out int distance)
    {
        var count = collider.Cast(Vector2.right * x, hits, ToUnits(Math.Abs(x)) + Const.CollisionOffset);
        for (var i = 0; i < count; i++)
        {
            if (!hits[i]) continue;
            distance = (hits[i].distance - Const.CollisionOffset).ToPixels() * Math.Sign(x);
            return true;
        }

        distance = x;
        return false;
    }

    public static bool CollideAtY(this BoxCollider2D collider, RaycastHit2D[] hits, int y, out int distance)
    {
        var count = collider.Cast(Vector2.up * y, hits, ToUnits(Math.Abs(y)) + Const.CollisionOffset);
        for (var i = 0; i < count; i++)
        {
            if (!hits[i]) continue;
            distance = (hits[i].distance - Const.CollisionOffset).ToPixels() * Math.Sign(y);
            return true;
        }

        distance = y;
        return false;
    }

    public static float ToUnits(this int pixels) => pixels * Const.PixelSize;
    public static float ToUnits(this float pixels) => pixels * Const.PixelSize;
    public static Vector2 ToUnits(this Vector2Int pixels) => new(pixels.x.ToUnits(), pixels.y.ToUnits());
    public static Vector2 ToUnits(this Vector2 pixels) => new(pixels.x.ToUnits(), pixels.y.ToUnits());
    public static Rect ToUnits(this RectInt pixels) => new(pixels.position.ToUnits(), pixels.size.ToUnits());
    public static Rect ToUnits(this Rect pixels) => new(pixels.position.ToUnits(), pixels.size.ToUnits());

    public static int ToPixels(this float units) => Mathf.RoundToInt(units / Const.PixelSize);
    public static Vector2Int ToPixels(this Vector2 units) => new(units.x.ToPixels(), units.y.ToPixels());
    public static Vector2Int ToPixels(this Vector3 units) => new(units.x.ToPixels(), units.y.ToPixels());
    public static RectInt ToPixels(this Rect units) => new(units.position.ToPixels(), units.size.ToPixels());
}