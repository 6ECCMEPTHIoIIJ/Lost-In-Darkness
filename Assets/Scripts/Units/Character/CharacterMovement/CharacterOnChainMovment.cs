using UnityEngine;

public class CharacterOnChainMovment : CharacterMovment
{
    [SerializeField] float climbSpeed;
    [SerializeField] float decentSpeed;
    [SerializeField] private float minJumpHeight = 3f;
    [SerializeField] private float jumpHeight = 6f;


    private float _jumpSpeed;
    private float _endJumpMultiplier;

    private Rigidbody2D _rb;
    private CharacterMovmentController _controller;

    public override void ProcessMoveY(bool isGrounded, float directtion)
    {
        _rb.linearVelocityX = 0f;
        _rb.linearVelocityY = directtion > 0 ? directtion * climbSpeed : directtion * decentSpeed;

        if (!collisionsList.Find((col) => col.GetComponent<ChainToClimb>() != null))
            _controller.SetNewCharacterState(CharacterStates.onGround);
    }

    override public void ProcessJump()
    {
        transform.parent = null;
        _rb.linearVelocityY = _jumpSpeed;
        _controller.SetNewCharacterState(CharacterStates.onGround);
    }

    override protected void InitializeJumpData()
    {
        _endJumpMultiplier = Mathf.Sqrt(minJumpHeight / jumpHeight);
        var gravity = -Physics2D.gravity.y;
        _jumpSpeed = Mathf.Sqrt(2f * jumpHeight * gravity) + gravity * Time.fixedDeltaTime;
    }

    private void Awake()
    {
        InitializeJumpData();
        _rb = GetComponent<Rigidbody2D>();
        _controller = GetComponent<CharacterMovmentController>();
        collisionsList = _controller.getCollisionsList();
    }
}
