using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    [SerializeField] int vertexCount = 20;
    [SerializeField] float lineWidth = 0.2f;
    [SerializeField] float radius;

    private LineRenderer mLineRenderer;
    private Player player;

    private void Start()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        player = GetComponentInParent<Player>();

        switch (player.Id)
        {
            case 0:
                mLineRenderer.startColor = Color.red;
                break;
            case 1:
                mLineRenderer.startColor = Color.green;
                break;
            case 2:
                mLineRenderer.startColor = Color.blue;
                break;
            case 3:
                mLineRenderer.startColor = Color.yellow;
                break;
            default:
                mLineRenderer.startColor = Color.cyan;
                break;
        }
        SetCircle();
    }

    private void SetCircle()
    {
        if (!mLineRenderer.enabled)
            mLineRenderer.enabled = true;
        mLineRenderer.widthMultiplier = lineWidth;

        radius = Vector3.Distance(Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMax, 0f)),
            Camera.main.ScreenToWorldPoint(new Vector3(0f, Camera.main.pixelRect.yMin, 0f))) * 0.5f - lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        mLineRenderer.positionCount = vertexCount;

        for (int i = 0; i < mLineRenderer.positionCount; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
            mLineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldpos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++)
        {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), radius * Mathf.Sin(theta), 0);
            Gizmos.DrawLine(oldpos, transform.position + pos);
            oldpos = transform.position + pos;

            theta += deltaTheta;
        }
    }
    #endif
}

