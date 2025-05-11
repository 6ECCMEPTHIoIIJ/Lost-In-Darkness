using UnityEngine;

public class FlyingPlatform : Solid
{
    [SerializeField] private float speed;
    
    private void FixedUpdate()
    {
        Move(speed * Time.fixedDeltaTime, 0);
    }
}