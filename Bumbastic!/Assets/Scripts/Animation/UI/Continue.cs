public class Continue : BaseAnimationUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// To hide continue UI
    /// </summary>
    public void Hide() { m_Animator.SetTrigger("Hide"); }
}
