using UnityEngine;

public class InGameCanvas : MonoBehaviour
{
    Animator m_Animator;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
    }

    private void SetEndAnimation()
    {
        m_Animator.SetBool("isGameOver", true);
    }
}
