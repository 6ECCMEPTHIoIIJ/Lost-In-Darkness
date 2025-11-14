using UnityEngine;
using UnityEngine.Windows;

public class CharacterGroundMovment : CharacterMovment
{
    private Rigidbody2D _rb;
    private CharacterMovmentController _controller;

    private float _jumpSpeed;
    private float _endJumpMultiplier;

    [SerializeField] private float minJumpHeight = 3f;
    [SerializeField] private float jumpHeight = 6f;

    [Header("In Air")][SerializeField] private float inAirSpeed = 0.0625f * 30;
    [SerializeField] private float maxFallSpeed = 9.81f * 4f;

    [Header("Run")][SerializeField] private float runSpeed = 0.0625f * 50;

    override public void ProcessJump()
    {
        _rb.linearVelocityY = _jumpSpeed;
    }

    override public void EndJump()
    {
        _rb.linearVelocityY *= _endJumpMultiplier;
    }

    override public void StopMovement()
    {
        _rb.linearVelocityX = 0;
        _rb.linearVelocityY = 0;
    }

    override public void ProcessMoveX(bool isGrounded, float direction)
    {
        var speed = isGrounded ? runSpeed : inAirSpeed;
        _rb.linearVelocityX = direction * speed;
    }

    public override void ProcessMoveY(bool isGrounded, float directtion)
    {
        Debug.Log(collisionsList.Count);
        if (directtion > 0)
            foreach (var ob in collisionsList)
            {
                ChainToClimb chain = ob.GetComponent<ChainToClimb>();
                if (chain)
                {
                    transform.SetParent(chain.transform, false);
                    transform.position = new Vector2(ob.transform.position.x, transform.position.y);
                    _controller.SetNewCharacterState(CharacterStates.onChain);
                }
            }
    }

    override protected void InitializeJumpData()
    {
        _endJumpMultiplier = Mathf.Sqrt(minJumpHeight / jumpHeight);
        var gravity = -Physics2D.gravity.y;
        _jumpSpeed = Mathf.Sqrt(2f * jumpHeight * gravity) + gravity * Time.fixedDeltaTime;
    }

    override public void ProcessFall()
    {
        _rb.linearVelocityY = Mathf.Max(-maxFallSpeed, _rb.linearVelocityY);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<CharacterMovmentController>();
        collisionsList = _controller.getCollisionsList();
    }

    private void Start()
    {
        InitializeJumpData();
    }
}
