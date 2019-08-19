using UnityEngine;

public class StarObtained : BaseAnimationUI
{
    UIParticleSystem cParticleSystem = null;
    [SerializeField] private float rotVel = 10f;

    protected override void Awake()
    {
        base.Awake();
        cParticleSystem = GetComponentInChildren<UIParticleSystem>(true);
    }

    private void Start()
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.starObtained, 1f);
    }

    private void Update()
    {
        cParticleSystem.transform.eulerAngles += Vector3.forward * Time.deltaTime * rotVel;
    }
}
