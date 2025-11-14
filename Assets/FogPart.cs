using System.Net;
using UnityEngine;

public class FogPart : MonoBehaviour
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float startScale = 0.1f;
    [SerializeField] private float scaleGrowSpeed = 0.3f;
    
    private float currentScale;

    private Vector3 targetPos;
    private float xSpeed;
    private float maxY;

    private bool isMovingToTargetpos = true;
    private bool isTargetPosRight;

    public void Initialize (Vector3 startPos, Vector3 endPos, float maxY)
    {
        transform.position = startPos;
        targetPos = endPos;
        isTargetPosRight = endPos.x > startPos.x;

        xSpeed = (endPos.x - startPos.x) / (endPos.y - startPos.y);
        this.maxY = maxY;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.localScale = Vector3.one * startScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 pos = transform.position;
        pos.y += speed * Time.deltaTime;

        if(currentScale < 1)
        {
            currentScale += scaleGrowSpeed * Time.deltaTime;
            transform.localScale = Vector3.one * currentScale;
        }

        if (isMovingToTargetpos)
        {
            pos.x += xSpeed * Time.deltaTime;
            if (pos.x > targetPos.x && isTargetPosRight ||
                pos.x < targetPos.x && !isTargetPosRight)
                isMovingToTargetpos = false;
        }

        if (pos.y > maxY)
            Destroy(this.gameObject);

        transform.position = pos;
    }
}
