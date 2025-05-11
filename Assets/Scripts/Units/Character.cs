using NaughtyAttributes;
using UnityEngine;

public class Character : Actor
{
    
    private static readonly int MoveXAnim = Animator.StringToHash("MoveX");
    private static readonly int GroundedAnim = Animator.StringToHash("Grounded");
    private static readonly int JumpAnim = Animator.StringToHash("Jump");
    private static readonly int FlipXAnim = Animator.StringToHash("FlipX");

    [ShowNonSerializedField] private bool _isGrounded;
    [ShowNonSerializedField] private float _velocityY;
    [ShowNonSerializedField] private float _velocityX;
    [ShowNonSerializedField] private bool _flipX;

    private InputReader _input;
    private Animator _anim;

    [SerializeField] private int gravity = 16 * 10;
    [SerializeField] private int inAirSpeed = 16 * 3;
    [SerializeField] private int runSpeed = 16 * 5;

    protected override void Awake()
    {
        base.Awake();
        _input = GetComponent<InputReader>();
        _anim = GetComponent<Animator>();
    }

    protected override void Start()
    {
        base.Start();
        _flipX = _input.FlipX;
    }

    private void Update()
    {
        _anim.SetBool(MoveXAnim, _input.MoveX != 0);
        _anim.SetBool(GroundedAnim, _isGrounded);
        if (_input.FlipX != _flipX)
        {
            _anim.SetTrigger(FlipXAnim);
            _flipX = _input.FlipX;
        }
    }

    private void FixedUpdate()
    {
        _velocityX = _input.MoveX * runSpeed;
        transform.FlipX(_input.FlipX);
        MoveX(_velocityX * Time.fixedDeltaTime, OnTouchingWall);
        _isGrounded = _isGrounded && CollideAtY(-1, out _);

        if (!_isGrounded)
        {
            _velocityY -= gravity * Time.fixedDeltaTime;
            MoveY(_velocityY * Time.fixedDeltaTime, OnLand);
        }
    }

    public override void Squish()
    {
        base.Squish();
        Destroy(gameObject);
    }

    private void OnLand()
    {
        _isGrounded = true;
        _velocityY = 0f;
        YRemainder = 0f;
    }

    private void OnTouchingWall()
    {
        XRemainder = 0f;
    }
}