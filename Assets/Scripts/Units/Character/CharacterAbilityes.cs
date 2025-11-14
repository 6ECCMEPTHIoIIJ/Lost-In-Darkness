using UnityEngine;

public class CharacterAbilityes : MonoBehaviour
{
    [SerializeField] KickZone kickZone;

    public void UseKick()
    {
        Instantiate(kickZone, transform.position + new Vector3(1 * transform.localScale.x > 0 ? 1 : -1, 0.5f, 0) , Quaternion.identity);
    }
}
