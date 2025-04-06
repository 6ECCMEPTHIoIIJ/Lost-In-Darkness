using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterInAirPhysicsManager : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private bool _prevFlipX;
    private bool _flipX;

    private float _beginWalkingTime;
    private float _endWalkingTime;
    private float _beginScareTime;
    private float _endScareTime;

    [SerializeField] private float fallingSpeed;
    [SerializeField] private float gravity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnJump()
    {
    }

    public void OnFall()
    {
        _rigidbody.linearVelocityY = Mathf.Max(-fallingSpeed, _rigidbody.linearVelocityY - gravity * Time.fixedDeltaTime);
        _rigidbody.linearVelocityX = 0;
    }

    public void OnLand()
    {
        _rigidbody.linearVelocityY = 0;
    }
}