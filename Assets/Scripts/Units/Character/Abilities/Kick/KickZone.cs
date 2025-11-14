using System.Collections.Generic;
using UnityEngine;

public class KickZone : MonoBehaviour
{
    private float maxLifetaime = 0.2f;
    private float currentLifetime = 0;
    private List<InteractableObject> alreadyInteractedObject = new List<InteractableObject>();

    // Update is called once per frame
    void Update()
    {
        if (currentLifetime > maxLifetaime)
            Destroy(gameObject);

        currentLifetime += Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        InteractableObject collidedObject = collision.gameObject.GetComponent<InteractableObject>();

        if (collidedObject != null && !alreadyInteractedObject.Contains(collidedObject))
        {
            Character c = FindFirstObjectByType<Character>();
            collidedObject.Interact(c.transform.localScale.x > 0);
            alreadyInteractedObject.Add(collidedObject);
        }
    }
}
