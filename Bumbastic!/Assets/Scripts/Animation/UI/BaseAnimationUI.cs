using UnityEngine;

public class BaseAnimationUI : MonoBehaviour
{
    protected Animator m_Animator = null;

    public event System.Action OnAnimationEnded = null;

    protected virtual void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Executed by animation
    /// </summary>
    public void OnEndAnimation() { OnAnimationEnded?.Invoke(); }
}
