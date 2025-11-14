using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BoxCollider2D))]
public class SmallDoor : SwitchableObject
{
    Animator _anim;
    BoxCollider2D _boxCol;
    ShadowCaster2D _shadowCast;
    private bool isDoorOpened = false;

    private static readonly int OpenTrigger = Animator.StringToHash("OpenTrigger");
    private static readonly int CloseTrigger = Animator.StringToHash("CloseTrigger");
    private static readonly int IsOpenedBool = Animator.StringToHash("IsOpen");

    private void SetIsOpenValue(bool newVal)
    {
        isDoorOpened = newVal;
        _anim.SetBool(IsOpenedBool, newVal);
        _boxCol.enabled = !newVal;
        if (_shadowCast && !newVal)
            _shadowCast.enabled = false;
    }

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _boxCol = GetComponent<BoxCollider2D>();
        _shadowCast = GetComponent<ShadowCaster2D>();
    }

    public void OpenDoor()
    {
        _anim.SetTrigger(OpenTrigger);
    }

    public void CloseDoor()
    {
        _anim.SetTrigger(CloseTrigger);
        if (_shadowCast)
            _shadowCast.enabled = true;
    }

    public void SetClose()
    {
        SetIsOpenValue(false);
    }

    public void SetOpen()
    {
        SetIsOpenValue(true);
    }

    public override void Switch()
    {
        if (isDoorOpened)
            CloseDoor();
        else
            OpenDoor();
    }
}
