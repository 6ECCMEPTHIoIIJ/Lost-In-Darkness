using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ChainToClimb : MonoBehaviour
{
    [SerializeField] GameObject chainPart;
    [SerializeField] int chainNumber;
    [SerializeField] float chainPartHeight;
    [SerializeField] float startHeight;

    private BoxCollider2D _coll;

    private List<ChainPart> chainParts = new List<ChainPart>();

    private void Awake()
    {
        _coll = GetComponent<BoxCollider2D>();

        for (int i = 0; i < chainNumber; i++)
        {
            Vector2 position = transform.position;
            position.y -= chainPartHeight * i + startHeight;

            GameObject chain = Instantiate(chainPart, position, Quaternion.identity);
            chain.transform.SetParent(transform);

            chainParts.Add(chain.GetComponent<ChainPart>());
        }

        if (chainParts.Count > 0)
            _coll.size = new Vector2(_coll.size.x / 2, Mathf.Abs(-chainParts.ElementAt(chainParts.Count - 1).transform.position.y + chainParts.ElementAt(0).transform.position.y) + chainPartHeight);

        _coll.offset = new Vector2(0, -_coll.size.y / 2);
    }
}
