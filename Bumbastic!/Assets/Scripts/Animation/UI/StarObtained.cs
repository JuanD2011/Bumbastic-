using UnityEngine;
using System.Collections;

public class StarObtained : BaseAnimationUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        StartCoroutine(SyncSFXAndVFX());
    }

    IEnumerator SyncSFXAndVFX()
    {
        AnimatorStateInfo currentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitUntil(() => currentStateInfo.normalizedTime >= 0.9);
    }
}
