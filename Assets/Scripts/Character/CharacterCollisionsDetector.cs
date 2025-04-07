using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterCollisionsDetector : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private Rect[] groundDetectRays;
    [SerializeField] private Color groundDetectColor;
    [SerializeField] private LayerMask groundLayer;

    public bool IsGrounded { get; private set; }
    public Vector2 GroundDetectedPosition { get; private set; }

    private void Awake()
    {
        _transform = transform;
    }

    private void FixedUpdate()
    {
        var characterPosition = new Vector2(_transform.position.x, _transform.position.y);
        foreach (var groundDetectRay in groundDetectRays)
        {
            var groundDetectOrigin = characterPosition + groundDetectRay.position;
            var groundDetectDirection = groundDetectRay.size;
            var groundDetectDistance = groundDetectDirection.magnitude;
            var groundHit =
                Physics2D.Raycast(groundDetectOrigin, groundDetectDirection, groundDetectDistance, groundLayer);
            IsGrounded = groundHit;
            GroundDetectedPosition = groundHit.point;
            if (IsGrounded) break;
        }
    }

    private void OnDrawGizmos()
    {
        if (_transform == null) return;

        Gizmos.color = groundDetectColor;
        foreach (var groundDetectRay in groundDetectRays)
        {
            var groundDetectFrom =
                _transform.position + new Vector3(groundDetectRay.position.x, groundDetectRay.position.y);
            var groundDetectTo = groundDetectFrom + new Vector3(groundDetectRay.size.x, groundDetectRay.size.y);
            Gizmos.DrawLine(groundDetectFrom, groundDetectTo);
        }

        if (IsGrounded)
        {
            var groundDetectedPosition = new Vector3(GroundDetectedPosition.x, GroundDetectedPosition.y);
            Gizmos.DrawWireSphere(groundDetectedPosition, 0.2f);
        }
    }
}