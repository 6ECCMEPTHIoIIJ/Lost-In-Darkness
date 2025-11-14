using UnityEngine;

public class CharacterMovmentController : CharacterMovment
{
    [SerializeField] CharacterGroundMovment characterOnGround;
    [SerializeField] CharacterOnChainMovment characterOnChain;

    private Rigidbody2D _rb;

    private CharacterMovment currentMovment;

    public CharacterStates currentState { get; private set; }

    public CharacterMovment GetCurrentMovment()
    {
        return currentMovment;
    }

    override public void ProcessJump()
    {
        currentMovment.ProcessJump();
    }

    override public void EndJump()
    {
        currentMovment.EndJump();
    }

    override public void StopMovement()
    {
        currentMovment.StopMovement();
    }

    override public void ProcessMoveX(bool isGrounded, float direction)
    {
        currentMovment.ProcessMoveX(isGrounded, direction);
    }

    public override void ProcessMoveY(bool isGrounded, float directtion)
    {
        currentMovment.ProcessMoveY(isGrounded, directtion);
    }

    override public void ProcessFall()
    {
        currentMovment.ProcessFall();
    }

    public void SetNewCharacterState(CharacterStates newState)
    {
        _rb.gravityScale = 1;
        currentState = newState;

        switch (newState)
        {
            case CharacterStates.onChain:
                {
                    _rb.gravityScale = 0;
                    currentMovment.enabled = false;
                    characterOnChain.enabled = true;
                    currentMovment = characterOnChain;
                    break;
                }

            default:
                {
                    currentMovment.enabled = false;
                    characterOnGround.enabled = true;
                    currentMovment = characterOnGround;
                    break;
                }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collisionsList.Add(collision.gameObject);    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collisionsList.Remove(collision.gameObject);
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        characterOnGround.enabled = true;
        currentMovment = characterOnGround;
    }
}
