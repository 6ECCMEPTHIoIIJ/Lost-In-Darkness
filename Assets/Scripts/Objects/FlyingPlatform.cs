using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class FlyingPlatform : MonoBehaviour
{
    [SerializeField] private float flashDuration = 2f;
    [SerializeField] private float speed = 2f;
    [SerializeField] private Vector2[] points;

    private Rigidbody2D _rb;
    private Transform _tr;
    private Light2D[] _lights;

    private bool[] _brights;
    private float _flashSpeed;
    private float _intensity;

    private int _currentPoint;

    private readonly SortedSet<Rigidbody2D> _passengers = new();

    private void Awake()
    {
        _tr = transform;
        _rb = GetComponent<Rigidbody2D>();
        _lights = GetComponentsInChildren<Light2D>();
    }

    private void Start()
    {
        _tr.position = points[_currentPoint];

        _intensity = _lights.Max(l => l.intensity);
        _brights = new bool[_lights.Length];
        for (var i = 0; i < _lights.Length; i++)
        {
            _lights[i].intensity = Random.Range(0f, _intensity);
            _brights[i] = Random.value > 0.5f;
        }

        _flashSpeed = _intensity / flashDuration;
    }

    private void Update()
    {
        ProcessAnimation();
    }

    private void ProcessAnimation()
    {
        for (var i = 0; i < _lights.Length; i++)
        {
            var light = _lights[i];
            var intensity = light.intensity;
            var bright = _brights[i];
            light.intensity += (bright ? _flashSpeed : -_flashSpeed) * Time.deltaTime;
            if (bright && intensity > _intensity
                || !bright && intensity < 0f)
            {
                _brights[i] = !bright;
            }
        }
    }

    private void FixedUpdate()
    {
        var origin = (Vector2)_tr.position;
        var destination = points[_currentPoint];
        var path = destination - origin;
        var distance = path.magnitude;
        if (distance < 0.1f)
        {
            _currentPoint = (_currentPoint + 1) % points.Length;
        }
        else
        {
            _rb.linearVelocity = path.normalized * Mathf.Min(speed, distance / Time.fixedDeltaTime);
        }

        foreach (var passenger in _passengers)
        {
            passenger.linearVelocityX += _rb.linearVelocityX;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_tr.position.y < other.transform.position.y)
        {
            _passengers.Add(other.rigidbody);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        _passengers.Remove(other.rigidbody);
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(FlyingPlatform))]
    public class FlyingPlatformEditor : Editor
    {
        private void OnSceneGUI()
        {
            var platform = (FlyingPlatform)target;
            var box = platform.GetComponent<BoxCollider2D>();
            var bounds = box.bounds;
            var offset = box.offset;
            for (var i = 0; i < platform.points.Length; i++)
            {
                EditorGUI.BeginChangeCheck();
                var prevPos = platform.points[i];
                var newPos = Handles.PositionHandle(prevPos, Quaternion.identity);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(platform, "Change FlyingPlatform Point");
                    platform.points[i] = newPos;
                }

                Handles.DrawWireCube(
                    platform.points[i] + offset, bounds.size);
            }

            Handles.BeginGUI();
            if (GUI.Button(new Rect(10, 10, 100, 30), "Add Point"))
            {
                Undo.RecordObject(platform, "Add Point");
                platform.points = platform.points.Append(platform.transform.position).ToArray();
            }

            Handles.EndGUI();
        }
    }

#endif
}