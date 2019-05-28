using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    [SerializeField] Settings settings = null;

    [SerializeField] int vertexCount = 40;
    [SerializeField] float lineWidth = 0.2f;
    [SerializeField] float radius = 0f;

    private LineRenderer mLineRenderer = null;
    private Player player;

    private void Start()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        player = GetComponentInParent<Player>();

        mLineRenderer.material.color = settings.playersColor[0];

        switch (player.Id)
        {
            case 0:
                mLineRenderer.material.color = settings.playersColor[0];
                break;
            case 1:
                mLineRenderer.material.color = settings.playersColor[1];
                break;
            case 2:
                mLineRenderer.material.color = settings.playersColor[2];
                break;
            case 3:
                mLineRenderer.material.color = settings.playersColor[3];
                break;
            default:
                mLineRenderer.material.color = settings.playersColor[0];
                break;
        }
        SetCircle();
    }

    private void SetCircle()
    {
        if (!mLineRenderer.enabled)
            mLineRenderer.enabled = true;
        mLineRenderer.widthMultiplier = lineWidth;

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

