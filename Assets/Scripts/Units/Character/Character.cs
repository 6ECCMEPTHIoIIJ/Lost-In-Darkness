using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CharacterAnimator))]
[RequireComponent(typeof(CharacterMovmentController))]
[RequireComponent(typeof(CharacterGroundMovment))]
[RequireComponent(typeof(CharacterAbilityes))]
public class Character : MonoBehaviour
{
    private Rigidbody2D _rb;
    private InputReader _input;
    private BoxCollider2D _box;
    private Transform _tr;
    private CharacterAnimator _anim;
    private CharacterMovmentController _move;
    private CharacterAbilityes _ability;

    private float _coyoteTime;

    private bool _prevFlipX;
    private bool _flipX;

    private bool _prevIsJumping;
    private bool _isJumping;
    private bool _canJump;

    private bool _prevIsGrounded;
    private bool _isGrounded;

    private bool _isBusy;

    private int _idleFrames;

    [SerializeField] private float coyoteDuration = 0.1f;

    [SerializeField] private float stopDelay = 0.1f;

    [Header("Collisions")]
    [SerializeField]
    private LayerMask whatIsGround;

    public void ToIdle()
    {
        _isBusy = false;
    }

    private void SetBuisy()
    {
        _isBusy = true;
        _move.StopMovement();
    }

    private void Awake()
    {
        _anim = GetComponent<CharacterAnimator>();
        _rb = GetComponent<Rigidbody2D>();
        _input = GetComponent<InputReader>();
        _box = GetComponent<BoxCollider2D>();
        _move = GetComponent<CharacterMovmentController>();
        _ability = GetComponent<CharacterAbilityes>();
        _tr = transform;
    }

    private void Update()
    {
        ProcessAnimation();
    }

    private void ProcessAnimation()
    {
        var maxIdleFrames = Mathf.RoundToInt(stopDelay / Time.deltaTime);
        _idleFrames = _input.MoveX == 0 ? _idleFrames + 1 : 0;
        _anim.SetMoveXBool(_idleFrames < maxIdleFrames);
        _anim.SetGroundedBool(_isGrounded);
        if (_flipX != _prevFlipX && _isGrounded)
        {
            _anim.SetFlipXTrigger();
        }

        if (_isJumping && !_prevIsJumping)
        {
            _anim.SetJumpTrigger();
        }

        _prevIsJumping = _isJumping;
    }

    private void FixedUpdate()
    {
        _anim.SetStateValue(_move.currentState.GetHashCode());

        if (!_isBusy)
        {
            switch (_move.currentState)
            {
                case CharacterStates.onGround:
                    ProcessMoveX();
                    ProcessMoveY();
                    ProcessFlipX();
                    ProcessJump();
                    ProcessKick();
                    CheckIfGrounded();
                    ProcessEndJump();
                    ProcessFall();
                    break;
                case CharacterStates.onChain:
                    _anim.SetCrawlUpSpeed(_rb.linearVelocityY);
                    ProcessMoveX();
                    ProcessMoveY();
                    ProcessFlipX();
                    ProcessJump();
                    break;
            }          
        }
    }

    private void ProcessMoveX()
    {
        _move.ProcessMoveX(_isGrounded, _input.MoveX);
    }

    private void ProcessMoveY()
    {
        _move.ProcessMoveY(_isGrounded, _input.MoveY);
    }

    private void ProcessJump()
    {
        if (_input.Jump &&  (_canJump &&  (_isGrounded || Time.fixedTime - _coyoteTime <= coyoteDuration) || _move.GetCurrentMovment().GetType() == typeof(CharacterOnChainMovment)))
        {
            _canJump = false;
            _isGrounded = false;
            _isJumping = true;
            _move.ProcessJump();
        }
    }

    private void ProcessKick()
    {
        if (!_isBusy && _input.Kick && _isGrounded)
        {
            SetBuisy();
            _anim.SetKickTrigger();
            _ability.UseKick();
        }
    }

    private void ProcessEndJump()
    {
        _isJumping &= !_isGrounded && _rb.linearVelocityY > 0.1f;
        if (!_isJumping) return;
        if (!_input.JumpHold)
        {
            _isJumping = false;
            _move.EndJump();
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
        _move.ProcessFall();
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