using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterCollisionsDetector : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private Vector2 groundDetectorPosition;
    [SerializeField] private float groundDetectorRadius;
    [SerializeField] private Color groundDetectorColor;
    [SerializeField] private LayerMask groundDetectLayer;

    public bool IsGrounded { get; private set; }

    private void Start()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        var detectorPosition = new Vector2(_transform.position.x, _transform.position.y) + groundDetectorPosition;
        IsGrounded = Physics2D.OverlapCircle(detectorPosition, groundDetectorRadius, groundDetectLayer);
        
    }

    private void OnDrawGizmos()
    {
        if (_transform == null) return;

        Gizmos.color = groundDetectorColor;
        Gizmos.DrawWireSphere(_transform.position + new Vector3(groundDetectorPosition.x, groundDetectorPosition.y, 0),
            groundDetectorRadius);
    }
}