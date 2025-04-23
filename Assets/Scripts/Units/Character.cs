using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(BoxCollider2D))]
public class Character : MonoBehaviour
{
    private static readonly int FlipXAnim = Animator.StringToHash("FlipX");
    private static readonly int MoveXAnim = Animator.StringToHash("MoveX");
    private static readonly int GroundedAnim = Animator.StringToHash("Grounded");
    private static readonly int JumpAnim = Animator.StringToHash("Jump");

    private Animator _anim;
    private Rigidbody2D _rb;
    private InputReader _input;
    private BoxCollider2D _box;
    private Transform _tr;

    private float _jumpSpeed;
    private float _endJumpMultiplier;

    private float _coyoteTime;

    private bool _prevFlipX;
    private bool _flipX;

    private bool _prevIsJumping;
    private bool _isJumping;
    private bool _canJump;

    private bool _prevIsGrounded;
    private bool _isGrounded;

    private bool _isStuck;
    private Rigidbody2D _stickyRb;

    private int _idleFrames;

    [Header("In Air")] [SerializeField] private float inAirSpeed = 0.0625f * 30;
    [SerializeField] private float maxFallSpeed = 9.81f * 4f;
    [SerializeField] private float jumpHeight = 6f;
    [SerializeField] private float minJumpHeight = 3f;
    [SerializeField] private float coyoteDuration = 0.1f;

    [Header("Run")] [SerializeField] private float runSpeed = 0.0625f * 50;
    [SerializeField] private float stopDelay = 0.1f;

    [Header("Collisions")] [SerializeField]
    private LayerMask whatIsGround;

    public void Stuck(Rigidbody2D rb)
    {
        _isStuck = true;
        _stickyRb = rb;
    }

    public void Unstuck()
    {
        _isStuck = false;
    }
    
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _input = GetComponent<InputReader>();
        _box = GetComponent<BoxCollider2D>();
        _tr = transform;
    }

    private void Start()
    {
        InitializeJumpData();
    }

    private void Update()
    {
        ProcessAnimation();
    }

    private void ProcessAnimation()
    {
        var maxIdleFrames = Mathf.RoundToInt(stopDelay / Time.deltaTime);
        _idleFrames = _input.MoveX == 0 ? _idleFrames + 1 : 0;
        _anim.SetBool(MoveXAnim, _idleFrames < maxIdleFrames);
        _anim.SetBool(GroundedAnim, _isGrounded);
        if (_flipX != _prevFlipX)
        {
            _anim.SetTrigger(FlipXAnim);
        }

        if (_isJumping && !_prevIsJumping)
        {
            _anim.SetTrigger(JumpAnim);
        }

        _prevIsJumping = _isJumping;
    }

    private void FixedUpdate()
    {
        ProcessMoveX();
        ProcessFlipX();
        ProcessJump();
        CheckIfGrounded();
        ProcessEndJump();
        ProcessFall();
    }

    private void InitializeJumpData()
    {
        _endJumpMultiplier = Mathf.Sqrt(minJumpHeight / jumpHeight);
        var gravity = -Physics2D.gravity.y;
        _jumpSpeed = Mathf.Sqrt(2f * jumpHeight * gravity) + gravity * Time.fixedDeltaTime;
    }

    private void ProcessMoveX()
    {
        var speed = _isGrounded ? runSpeed : inAirSpeed;
        _rb.linearVelocityX = _input.MoveX * speed;
        if (_isStuck)
        {
            _rb.linearVelocity += _stickyRb.linearVelocity;
        }
    }

    private void ProcessJump()
    {
        if (_canJump && _input.Jump && (_isGrounded || Time.fixedTime - _coyoteTime <= coyoteDuration))
        {
            _canJump = false;
            _isGrounded = false;
            _isJumping = true;
            _rb.linearVelocityY = _jumpSpeed;
        }
    }

    private void ProcessEndJump()
    {
        _isJumping &= !_isGrounded && _rb.linearVelocityY > 0.1f;
        if (!_isJumping) return;
        if (!_input.JumpHold)
        {
            _isJumping = false;
            _rb.linearVelocityY *= _endJumpMultiplier;
        }
        else if (_box.RaycastUp(whatIsGround))
        {
            _isJumping = false;
        }
    }

    private void ProcessFlipX()
    {
        _prevFlipX = _flipX;
        _flipX = _input.FlipX;
        if (_flipX != _prevFlipX)
        {
            _tr.FlipX(_flipX);
        }
    }

    private void ProcessFall()
    {
        _rb.linearVelocityY = Mathf.Max(-maxFallSpeed, _rb.linearVelocityY);
    }

    private void CheckIfGrounded()
    {
        _prevIsGrounded = _isGrounded;
        _isGrounded = !_isJumping && _box.RaycastDown(whatIsGround);
        _canJump |= _isGrounded;
        if (!_isGrounded && _prevIsGrounded)
        {
            _coyoteTime = Time.fixedTime;
        }
    }
}