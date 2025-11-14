using UnityEngine;

public class ChainPart : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    
    SpriteRenderer _spriteRend;

    private void Awake()
    {
        _spriteRend = GetComponent<SpriteRenderer>();

        if(_spriteRend != null)
        {
            _spriteRend.sprite = sprites[Random.Range(0, sprites.Length - 1)];
        }
    }
}
