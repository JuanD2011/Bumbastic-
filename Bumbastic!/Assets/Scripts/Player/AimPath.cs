using UnityEngine;

public class AimPath : MonoBehaviour
{
    Player m_Player;
    LineRenderer m_LineRenderer;
    private float targetRotation;
    private float turnSmoothVel;

    void Start()
    {
        m_Player = GetComponentInParent<Player>();
        m_LineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (m_Player.InputAiming != Vector2.zero)
        {
            if (!m_LineRenderer.enabled)
            {
                m_LineRenderer.enabled = true;
            }
            targetRotation = Mathf.Atan2(m_Player.InputAiming.x, m_Player.InputAiming.y) * Mathf.Rad2Deg;
            Debug.Log(targetRotation);
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