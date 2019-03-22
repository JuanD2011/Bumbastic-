using UnityEngine;

public class AimPath : MonoBehaviour
{
    Player m_Player;
    LineRenderer m_LineRenderer;
    private float targetRotation;
    private float turnSmoothVel;

    Vector2 aimNormalized;

    void Start()
    {
        m_Player = GetComponentInParent<Player>();
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        aimNormalized = m_Player.InputAiming.normalized;
        if (aimNormalized != Vector2.zero)
        {
            if (!m_LineRenderer.enabled)
            {
                m_LineRenderer.enabled = true;
            }
            targetRotation = -1f * Mathf.Atan2(aimNormalized.y, aimNormalized.x) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVel, m_Player.TurnSmooth);
        }
        else
        {
            if (m_LineRenderer.enabled)
            {
                m_LineRenderer.enabled = false;
            }
        }
    }
}