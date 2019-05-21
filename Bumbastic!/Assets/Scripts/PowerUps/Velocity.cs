using UnityEngine;
using System.Collections;

public class Velocity : PowerUp
{
    GameObject speedUpParticle;

    protected override void Start()
    {
        base.Start();
        Duration = 5f;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.cSpeedUP, 1f);
        StartCoroutine(InitSpeedUP());
        speedUpParticle.GetComponent<SpeedUpParticle>().OnComplete += OnComplete;
    }

    IEnumerator InitSpeedUP()
    {
        speedUpParticle = Instantiate(GameManager.Manager.speedUpParticleSystem, transform.position, Quaternion.identity, player.transform);
        player.speedPU = true;
        AudioManager.instance.PlaySFx(AudioManager.instance.audioClips.speedUP, 0.7f);
        yield return new WaitForSeconds(Duration);
        player.speedPU = false;
    }

    private void OnComplete()
    {
        Destroy(speedUpParticle.gameObject);
        Destroy(this);
    }
}
