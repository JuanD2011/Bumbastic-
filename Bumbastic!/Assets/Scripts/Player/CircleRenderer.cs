using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    [SerializeField] Settings settings = null;

    [SerializeField] int vertexCount = 40;
    [SerializeField] float lineWidth = 0.2f;
    [SerializeField] float radius = 0f;

    private LineRenderer mLineRenderer = null;
    private Player player;

    private void Awake()
    {
        mLineRenderer = GetComponent<LineRenderer>();
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        mLineRenderer.material.color = settings.playersColor[0];
        SetColor();
        SetCircle();
    }

    private void SetColor()
    {
        if (GameModeDataBase.IsCurrentBasesGame())
        {
            foreach (Player _player in BasesGameManager.basesGame.Teams[0].Members)
            {
                if (_player == player)
                {
                    mLineRenderer.material.color = BasesGameManager.basesGame.Teams[0].TeamColor;
                    return;
                } 
            }
            mLineRenderer.material.color = BasesGameManager.basesGame.Teams[1].TeamColor;
            return;
        }

        switch (player.Id)
        {
            case 0: mLineRenderer.material.color = settings.playersColor[0]; break;
            case 1: mLineRenderer.material.color = settings.playersColor[1]; break;
            case 2: mLineRenderer.material.color = settings.playersColor[2]; break;
            case 3: mLineRenderer.material.color = settings.playersColor[3]; break;
            default: mLineRenderer.material.color = settings.playersColor[0]; break;
        }
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
}
