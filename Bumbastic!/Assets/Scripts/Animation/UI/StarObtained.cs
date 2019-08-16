using UnityEngine;
using System.Collections;

public class StarObtained : BaseAnimationUI
{
    ParticleSystem cParticleSystem = null;
    [SerializeField] private float rotVel = 10f;

    protected override void Awake()
    {
        base.Awake();
        cParticleSystem = GetComponentInChildren<ParticleSystem>();
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
        AnimatorStateInfo currentStateInfo = m_Animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitUntil(() => currentStateInfo.normalizedTime >= 0.9);
    }
}
