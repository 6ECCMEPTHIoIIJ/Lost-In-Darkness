using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class FogSorce : MonoBehaviour
{
    [SerializeField] FogPart fogPart;

    [SerializeField] float startWidth = 1;
    [SerializeField] float endWidth = 2;

    float startDeltaLength = 0;
    float startTrailLength = 1;
    float maxTrailLength = 3;

    float timeForParticle = 0.1f;
    float particleCooldown = 0;

    private void CreateFogPart()
    {
        float positionScale = Random.Range(0, 1.0f);

        Vector3 startpos = transform.position;
        startpos.x += startWidth * (positionScale - 0.5f);
        startpos.y += startDeltaLength;

        Vector3 endpos = transform.position;
        endpos.x += endWidth * (positionScale - 0.5f);
        endpos.y += startTrailLength;

        FogPart part = Instantiate(fogPart, transform);

        part.Initialize(startpos, endpos, transform.position.y + maxTrailLength);
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(particleCooldown >= timeForParticle)
        {
            particleCooldown = 0;
            CreateFogPart();
        }
        else
        {
            particleCooldown += Time.deltaTime;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(FogSorce))]
    public class FogSourceEditor : Editor
    {

        private void StartPointHandleProcessing(FogSorce fogSource)
        {
            EditorGUI.BeginChangeCheck();

            Vector2 newStartPos = fogSource.transform.position;
            newStartPos.y += fogSource.startDeltaLength;

            float handleSize = HandleUtility.GetHandleSize(newStartPos) * 0.3f;
            Vector2 startPosHandle = Handles.Slider(newStartPos, Vector2.up, handleSize, Handles.ArrowHandleCap, 0f);

            newStartPos.y = startPosHandle.y > fogSource.transform.position.y ? startPosHandle.y : fogSource.transform.position.y;
            newStartPos.y = newStartPos.y < fogSource.transform.position.y + fogSource.startTrailLength ? newStartPos.y : fogSource.transform.position.y + fogSource.startTrailLength;

            fogSource.startDeltaLength = newStartPos.y - fogSource.transform.position.y;

            Handles.DrawWireCube(newStartPos, new Vector2(fogSource.startWidth, 0.1f));
            EditorGUI.EndChangeCheck();
        }

        private void StartTrailPointHandleProcessing(FogSorce fogSource)
        {
            EditorGUI.BeginChangeCheck();

            Vector2 newEndPos = fogSource.transform.position;
            newEndPos.y += fogSource.startTrailLength;

            float handleSize = HandleUtility.GetHandleSize(newEndPos) * 0.4f;
            Vector2 endPosHandle = Handles.Slider(newEndPos, Vector2.up, handleSize, Handles.ArrowHandleCap, 0f);

            newEndPos.y = endPosHandle.y > fogSource.transform.position.y + fogSource.startDeltaLength ? endPosHandle.y : fogSource.transform.position.y + fogSource.startDeltaLength;
            newEndPos.y = newEndPos.y < fogSource.transform.position.y + fogSource.maxTrailLength ? newEndPos.y : fogSource.transform.position.y + fogSource.maxTrailLength;

            fogSource.startTrailLength = newEndPos.y - fogSource.transform.position.y;
            Handles.DrawWireCube(newEndPos, new Vector2(fogSource.endWidth, 0.1f));
            EditorGUI.EndChangeCheck();
        }

        private void EndTrailPointHandleProcessing(FogSorce fogSource)
        {
            EditorGUI.BeginChangeCheck();

            Vector2 newEndPos = fogSource.transform.position;
            newEndPos.y += fogSource.maxTrailLength;

            float handleSize = HandleUtility.GetHandleSize(newEndPos) * 0.5f;
            Vector2 endPosHandle = Handles.Slider(newEndPos, Vector2.up, handleSize, Handles.ArrowHandleCap, 0f);

            newEndPos.y = endPosHandle.y > fogSource.transform.position.y + fogSource.startTrailLength ? endPosHandle.y : fogSource.transform.position.y + fogSource.startTrailLength;

            fogSource.maxTrailLength = newEndPos.y - fogSource.transform.position.y;
            Handles.DrawWireCube(newEndPos, new Vector2(fogSource.endWidth, 0.1f));
            EditorGUI.EndChangeCheck();
        }

        private void OnSceneGUI()
        {
            var fogSource = (FogSorce)target;

            StartPointHandleProcessing(fogSource);

            StartTrailPointHandleProcessing(fogSource);

            EndTrailPointHandleProcessing(fogSource);
        }
    }
#endif
}
