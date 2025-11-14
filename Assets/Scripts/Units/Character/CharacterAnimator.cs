using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private static readonly int FlipXAnim = Animator.StringToHash("FlipX");
    private static readonly int MoveXAnim = Animator.StringToHash("MoveX");
    private static readonly int GroundedAnim = Animator.StringToHash("Grounded");
    private static readonly int JumpAnim = Animator.StringToHash("Jump");
    private static readonly int KickAim = Animator.StringToHash("Kick");
    private static readonly int CharacterSateCode = Animator.StringToHash("CharacterSateCode");
    private static readonly int CrawlAnim = Animator.StringToHash("CrawlSpeed");
    private static readonly int IsCrawlingUpAnim = Animator.StringToHash("IsCrawlingUp");

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void SetMoveXBool(bool value)
    {
        _anim.SetBool(MoveXAnim, value);
    }

    public void SetGroundedBool(bool value)
    {
        _anim.SetBool(GroundedAnim, value);
    }

    public void SetFlipXTrigger()
    {
        _anim.SetTrigger(FlipXAnim);
    }

    public void SetJumpTrigger()
    {
        _anim.SetTrigger(JumpAnim);
    }

    public void SetKickTrigger()
    {
        _anim.SetTrigger(KickAim);
    }

    public void SetStateValue(int value)
    {
        _anim.SetInteger(CharacterSateCode, value);
    }

    public void SetCrawlUpSpeed(float value)
    {
        _anim.SetFloat(CrawlAnim, value);
        _anim.SetFloat(IsCrawlingUpAnim, value > 0 ? 1: 0);
    }
}
