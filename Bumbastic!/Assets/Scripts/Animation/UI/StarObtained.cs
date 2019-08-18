using UnityEngine;
using System.Collections;

public class StarObtained : BaseAnimationUI
{
    UIParticleSystem cParticleSystem = null;
    [SerializeField] private float rotVel = 10f;

    protected override void Awake()
    {
        base.Awake();
        cParticleSystem = GetComponentInChildren<UIParticleSystem>();
    }

    private void Start()
    {
        StartCoroutine(SyncSFXAndVFX());
    }

    private void Update()
    {
        cParticleSystem.transform.eulerAngles += Vector3.forward * Time.deltaTime * rotVel;
    }

    IEnumerator SyncSFXAndVFX()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.starObtained, 1f);
        AnimatorStateInfo currentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);
        yield return new WaitUntil(() => currentStateInfo.normalizedTime >= 0.24f);
        //TODO activate particles
    }
}
