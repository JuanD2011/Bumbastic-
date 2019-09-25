using UnityEngine;

public class TangleExplosionVFX : MonoBehaviour
{
    private Animator distortionAnimator = null;

    private void Awake() => distortionAnimator = GetComponentInChildren<Animator>();

    private void Update()
    {
        if (distortionAnimator.GetCurrentAnimatorStateInfo(0).IsName("Implosion") && distortionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.99f)
        {
            Destroy(gameObject);
        }
    }

    public void Explosion(float _duration)
    {
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.tangleExplosion, 1f);
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.stun, 1f);
        AudioManager.instance.ChangeSnapshot(AudioManager.instance.audioClips.tangledSnapshot);
        distortionAnimator.SetTrigger("Explosion");
        Invoke("Implosion", _duration);
    }

    private void Implosion()
    {
        AudioManager.instance.ChangeSnapshot(AudioManager.instance.audioClips.normalSnapshot);
        distortionAnimator.SetTrigger("Implosion");
    }
}
