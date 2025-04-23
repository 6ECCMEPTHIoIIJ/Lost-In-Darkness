using UnityEngine;

public static class UnityEx
{
    public static void FlipX(this Transform tr, bool flipX)
    {
        tr.localScale = new Vector3(Mathf.Abs(tr.localScale.x) * (flipX ? -1f : 1f), tr.localScale.y, tr.localScale.z);
    }

    public static bool RaycastDown(this BoxCollider2D box, LayerMask targetLayer)
    {
        var bounds = box.bounds;
        var leftOrigin = new Vector2(bounds.min.x, bounds.min.y);
        var rightOrigin = new Vector2(bounds.max.x, bounds.min.y);
        return Physics2D.Raycast(leftOrigin, Vector2.down, 0.1f, targetLayer) ||
               Physics2D.Raycast(rightOrigin, Vector2.down, 0.1f, targetLayer);
    }
    
    public static bool RaycastUp(this BoxCollider2D box, LayerMask targetLayer)
    {
        var bounds = box.bounds;
        var leftOrigin = new Vector2(bounds.min.x, bounds.max.y);
        var rightOrigin = new Vector2(bounds.max.x, bounds.max.y);
        return Physics2D.Raycast(leftOrigin, Vector2.up, 0.1f, targetLayer) ||
               Physics2D.Raycast(rightOrigin, Vector2.up, 0.1f, targetLayer);
    }
}