using UnityEngine;

public class AimPath : MonoBehaviour
{
    Player m_Player;
    LineRenderer m_LineRenderer;
    private float targetRotation;

    void Start()
    {
        m_Player = GetComponentInParent<Player>();
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (m_Player.HasBomb)
        {
            if (m_Player.InputAiming != Vector2.zero)
            {
                transform.LookAt(m_Player.InputAiming); 
                m_LineRenderer.enabled = true;
            }
            else
            {
                m_LineRenderer.enabled = false;
            }
        }
    }
}
