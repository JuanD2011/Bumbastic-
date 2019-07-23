using UnityEngine;

public class CanvasPodium : CanvasBase
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        PlayerMenu.OnStartButton += StartButton;
    }

    #region Animation events
    public void OnLoadScreenComplete()
    {
        StartCoroutine(OnLoadScene?.Invoke("Menu"));//Lvl Manager hears it.
    }
    #endregion

    private void StartButton(byte _id)
    {
        AnimatorStateInfo animatorStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        m_Animator.SetTrigger("loadingScreen");
    }
}
