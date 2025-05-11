using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

public class Units : MonoBehaviour
{
    private readonly Dictionary<int, Actor> _actors = new();

    private static Units _instance;

    private static Units Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameObject("Units").AddComponent<Units>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static IEnumerable<Actor> AllActors => Instance._actors.Values;

    [ShowNativeProperty] private static int ActorsCount => Instance._actors.Count;

    public static void AddActor(Actor actor)
    {
        Instance._actors.Add(actor.GetInstanceID(), actor);
    }

    public static void RemoveActor(Actor actor)
    {
        Instance._actors.Remove(actor.GetInstanceID());
    }

    public static IEnumerable<Actor> GetAllRidingActors(Solid solid)
    {
        return AllActors.Where(actor => actor.IsRiding(solid));
    }
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public abstract class Unit : MonoBehaviour
{
    private Rigidbody2D _rb;

    [SerializeField]
    [OnValueChanged(nameof(OnBoundsChanged))]
    [ValidateInput(nameof(HasBounds), "Bounds must have positive area")]
    protected RectInt bounds;

    [ShowNonSerializedField] protected float XRemainder;
    [ShowNonSerializedField] protected float YRemainder;
    protected BoxCollider2D Collider;

    public RectInt Bounds { get; private set; }

    protected virtual void Awake()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        Collider = gameObject.GetComponent<BoxCollider2D>();
        OnBoundsChanged();
    }

    protected virtual void Start()
    {
        _rb.bodyType = RigidbodyType2D.Static;
        UpdateBounds();
    }

    protected virtual void OnDrawGizmos()
    {
        var center = (Vector2)transform.position + bounds.center.ToUnits();
        var size = bounds.size.ToUnits();
        Gizmos.DrawWireCube(center, size);
    }

    protected void Move(Vector2 direction, int distance)
    {
        transform.position += (Vector3)direction * distance.ToUnits();
        UpdateBounds();
    }

    private void UpdateBounds()
    {
        Bounds = new RectInt(
            transform.position.ToPixels() + bounds.position,
            bounds.size
        );
    }

    protected void EnableCollision()
    {
        Collider.enabled = true;
    }

    protected void DisableCollision()
    {
        Collider.enabled = false;
    }

    protected virtual void OnBoundsChanged()
    {
    }

    private bool HasBounds()
    {
        return bounds is { width: > 0, height: > 0 };
    }
}

public abstract class Solid : Unit
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = LayerMask.NameToLayer("Solid");

        Collider.FromBounds(bounds.ToUnits());
    }
    
    public void Move(float xUnits, float yUnits)
    {
        XRemainder += xUnits;
        YRemainder += yUnits;
        var stepsX = Mathf.RoundToInt(XRemainder);
        var stepsY = Mathf.RoundToInt(YRemainder);
        if (stepsX == 0 && stepsY == 0) return;

        var riding = Units.GetAllRidingActors(this).ToArray();

        DisableCollision();

        if (stepsX != 0)
        {
            XRemainder -= stepsX;
            Move(Vector2.right, stepsX);
            var actors = Units.AllActors.ToArray();
            foreach (var actor in actors)
            {
                if (OverlapCheck(actor))
                {
                    var distance = stepsX > 0
                        ? Bounds.xMax - actor.Bounds.xMin
                        : Bounds.xMin - actor.Bounds.xMax;
                    actor.MoveX(distance, actor.Squish);
                }
                else if (riding.Contains(actor))
                {
                    actor.MoveX(stepsX, null);
                }
            }
        }

        if (stepsY != 0)
        {
            YRemainder -= stepsY;
            Move(Vector2.up, stepsY);
            var actors = Units.AllActors.ToArray();
            foreach (var actor in actors)
            {
                if (OverlapCheck(actor))
                {
                    var distance = stepsY > 0
                        ? Bounds.yMax - actor.Bounds.yMin
                        : Bounds.yMin - actor.Bounds.yMax;
                    actor.MoveY(distance, actor.Squish);
                }
                else if (riding.Contains(actor))
                {
                    actor.MoveY(stepsY, null);
                }
            }
        }

        EnableCollision();
    }

    private bool OverlapCheck(Actor actor)
    {
        return actor.Bounds.xMin < Bounds.xMax
               && Bounds.xMin < actor.Bounds.xMax
               && actor.Bounds.yMin < Bounds.yMax
               && Bounds.yMin < actor.Bounds.yMax;
    }
}

public abstract class Actor : Unit
{
    private readonly RaycastHit2D[] _hits = new RaycastHit2D[2];

    [SerializeField] [ReadOnly] private Rect colliderBounds;

    protected override void Awake()
    {
        base.Awake();
        gameObject.layer = LayerMask.NameToLayer("Actor");

        OnBoundsChanged();
        Collider.FromBounds(colliderBounds);
    }

    protected virtual void OnEnable()
    {
        Units.AddActor(this);
    }

    protected virtual void OnDisable()
    {
        Units.RemoveActor(this);
    }

    public void MoveX(float amount, Action onCollide)
    {
        XRemainder += amount;
        var steps = Mathf.RoundToInt(XRemainder);
        if (steps == 0) return;

        XRemainder -= steps;
        var collide = CollideAtX(steps, out var distance);
        Move(Vector2.right, distance);
        if (collide) onCollide?.Invoke();
    }

    public void MoveY(float amount, Action onCollide)
    {
        YRemainder += amount;
        var steps = Mathf.RoundToInt(YRemainder);
        if (steps == 0) return;

        YRemainder -= steps;
        var collide = CollideAtY(steps, out var distance);
        Move(Vector3.up, distance);
        if (collide) onCollide?.Invoke();
    }

    public virtual bool IsRiding(Solid solid)
    {
        return solid.Bounds.xMin < Bounds.xMax
               && Bounds.xMin < solid.Bounds.xMax
               && Mathf.Approximately(solid.Bounds.yMax, Bounds.yMin);
    }

    public virtual void Squish()
    {
    }

    protected bool CollideAtX(int x, out int distance)
    {
        return Collider.CollideAtX(_hits, x, out distance);
    }

    protected bool CollideAtY(int y, out int distance)
    {
        return Collider.CollideAtY(_hits, y, out distance);
    }

    protected override void OnBoundsChanged()
    {
        colliderBounds.center = bounds.center.ToUnits();
        colliderBounds.size = (bounds.size - Vector2Int.one * 2).ToUnits();
    }
}