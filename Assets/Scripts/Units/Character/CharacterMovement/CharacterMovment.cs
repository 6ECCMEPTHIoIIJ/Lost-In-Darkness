using System.Collections.Generic;
using UnityEngine;

public class CharacterMovment : MonoBehaviour
{
    protected List<GameObject> collisionsList = new List<GameObject>();

    public virtual List<GameObject> getCollisionsList()
    {
        return collisionsList;
    }

    public virtual void ProcessJump() { }

    public virtual void EndJump() { }

    public virtual void StopMovement() { }

    public virtual void ProcessMoveX(bool isGrounded, float direction) { }

    public virtual void ProcessMoveY(bool isGrounded, float directtion) { }

    protected virtual void InitializeJumpData() { }

    virtual public void ProcessFall() { }
}
